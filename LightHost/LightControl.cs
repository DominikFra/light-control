using System;
using System.Timers;
using LsHardDevKit;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LightHost
{
    public class LightControl : IDisposable
    {
        public const int lightTick = 40;

        public const int ChannelCount = 128;
        private Timer _timer = new Timer(lightTick);
        private byte[] lightBuffer = new byte[ChannelCount];
        private Action<string> log;
        private Device device;
        private DmxUniverse universe;

        public LightControl(Action<string> log)
        {
            this.log = log;
            _timer.Elapsed += updateLight;
            _timer.Start();

            DmxInterface.init(Protocols.DASUSB);
            loadInterface();
            lightBuffer[18] = 255;
        }

        public IEnumerable<byte> Current()
        {
            return new List<byte>(this.lightBuffer);
        }

        public void SetChannelValue(int channel, byte value)
        {
            if (channel < ChannelCount)
                lightBuffer[channel] = value;
        }
        //public void Black()
        //{
        //    for (int i = 0; i < ChannelCount; i++)
        //        if(i != 6)
        //            lightBuffer[i] = 0;
        //}

        private void loadInterface()
        {
            if (!DmxInterface.enumerate())
            {
                log("Error Enumerate: " + DmxInterface.getLastError());
                return;
            }
            if(DmxInterface.getDeviceCount() > 0)
            {
                var device = DmxInterface.getDevice(0);
                string name;
                string typeName;
                if (device.getTypeName(out typeName) && device.getName(out name))
                    log("Device " + name + " [" + typeName + "] found");
                this.device = device;
                if (!device.openDevice())
                    log("Error Open: " + DmxInterface.getLastError());
                this.universe = device.getDmxUnivers(0);
                universe.configureAs(DmxUniverse.WorkingMode.DmxOut);
            }
        }

        private void updateLight(object sender, ElapsedEventArgs e)
        {
            if (universe == null)
                return;
            universe.sendDmx(this.lightBuffer);
        }

        public void Dispose()
        {
            _timer.Stop();
            if (device != null)
                device.close();
            DmxInterface.deinit();
        }
    }
}