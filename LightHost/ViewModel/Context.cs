using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LightHost.ViewModel
{
    public class Context : ViewModelBase
    {
        internal readonly LightControl Light;
        internal TaskScheduler uiThreadScheduler;
        internal Dispatcher uiDispatcher;

        public LightCommandsViewModel LightCommands { get; private set; }
        public Context()
        {
            Light = new LightControl(log);
            LightCommands = new LightCommandsViewModel(this);
            uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            uiDispatcher = Dispatcher.CurrentDispatcher;
            intro.Open(new Uri(Environment.CurrentDirectory + "/music/01_intro.mp3"));
            night.Open(new Uri(Environment.CurrentDirectory + "/music/04_night.mp3"));
            breakStart.Open(new Uri(Environment.CurrentDirectory + "/music/05_pause.mp3"));
            breakEnd.Open(new Uri(Environment.CurrentDirectory + "/music/06_pauseEnde.wav"));
        }

        private void log(string text)
        {
            MessageBox.Show(text);
        }


        internal void RefreshLights()
        {
            uiDispatcher.Invoke(() =>
            {
                var values = LightCommands.LightGroups.Select(x => x.CurrentValues).ToArray();
                var finalValues = new byte[LightControl.ChannelCount];
                for (int i = 0; i < LightControl.ChannelCount; i++)
                    finalValues[i] = values.Select(x => x[i]).Max();

                var explicits = LightCommands.LightGroups.Where(x => x.Explicit);
                foreach (var x in explicits)
                {
                    var lights = x.getLight();
                    foreach (var y in lights)
                        finalValues[y.Key] = y.Value;
                }
                for (int i = 0; i < LightControl.ChannelCount; i++)
                    Light.SetChannelValue(i, finalValues[i]);
                RaisePropertyChanged("CurrentLight");
            });
        }
        internal void RaiseCurrentLightChanged()
        {
            RaisePropertyChanged("CurrentLight");
        }
        public IEnumerable<byte> CurrentLight { get { return Light.Current().Take(20); } }

        public Dictionary<int, byte> behindStage = new Dictionary<int, byte>() { { 6, 110 } };

        public Dictionary<int, byte> stageFront = new Dictionary<int, byte>() { { 0, 128 }, { 4, 127 }, { 1, 129 }, { 5, 127 }, { 9, 197 }, { 10, 199 } };
        public Dictionary<int, byte> music = new Dictionary<int, byte>() { { 14, 150 } };
        public Dictionary<int, byte> doorRight = new Dictionary<int, byte>() { { 15, 171 } };
        public Dictionary<int, byte> street2 = new Dictionary<int, byte>() { { 10, 100 }, { 12, 167 }, { 15, 188 } };
        public Dictionary<int, byte> street = new Dictionary<int, byte>() { { 3, 185 }, { 12, 171 } };
        public Dictionary<int, byte> streetFront = new Dictionary<int, byte>() { { 2, 141 } };
        public Dictionary<int, byte> breakLight = new Dictionary<int, byte>() { { 7, 76 } };

        public Dictionary<int, byte> roomLeft = new Dictionary<int, byte>() { { 0, 178 }, { 4, 177 }, { 9, 138} };
        public Dictionary<int, byte> roomLeftFront = new Dictionary<int, byte>() { { 9, 71 } };
        public Dictionary<int, byte> cosmeStairs = new Dictionary<int, byte>() { { 12, 175 } };

        public Dictionary<int, byte> entranceLight = new Dictionary<int, byte>() { { 19, 79 } };
        public Dictionary<int, byte> roomRight = new Dictionary<int, byte>() { { 1, 166 }, { 5, 200 }, { 10, 138 } };
        public Dictionary<int, byte> blue = new Dictionary<int, byte>() { { 8, 81 }, { 10, 56 } };
        public Dictionary<int, byte> spot = new Dictionary<int, byte>() { { 1, 49 }, { 5, 79 }, { 16, 147 } };
        public Dictionary<int, byte> gapLight = new Dictionary<int, byte>() { { 17, 162 } };
        public MediaPlayer intro = new MediaPlayer();
        public MediaPlayer night = new MediaPlayer();
        public MediaPlayer breakStart = new MediaPlayer();
        public MediaPlayer breakEnd = new MediaPlayer();
    }
}
