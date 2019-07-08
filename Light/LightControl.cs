using System;
using System.Timers;
using LsHardDevKit;
using System.Collections.Generic;

namespace Light
{
    public class LightControl : IDisposable
    {
        public const int lightTick = 40;

        public const int ChannelCount = 128;
        private Timer _timer = new Timer(lightTick);
        private byte[] lightBuffer = new byte[ChannelCount];
        private Device device;
        private DmxUniverse universe;

        public LightControl()
        {
            _timer.Elapsed += updateLight;
            _timer.Start();

            DmxInterface.init(Protocols.DASUSB);
            loadInterface();
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

        private void loadInterface()
        {
            DmxInterface.enumerate();
            if (DmxInterface.getDeviceCount() > 0)
            {
                var device = DmxInterface.getDevice(0);
                //string name;
                //string typeName;
                //if (device.getTypeName(out typeName) && device.getName(out name))
                //    log("Device " + name + " [" + typeName + "] found");
                this.device = device;
                var success = device.openDevice();
                //if (!success)
                //    log("Error Open: " + DmxInterface.getLastError());
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