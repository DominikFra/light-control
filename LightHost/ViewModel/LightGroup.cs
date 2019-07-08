using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LightHost.ViewModel
{
    public class LightGroup : ContextViewModelBase
    {
        private string name;
        private Dictionary<int, byte> lights;
        private double value;

        public LightGroup(Context context, string name, Dictionary<int, byte> lights) : base(context)
        {
            this.name = name;
            this.lights = lights;
            CurrentValues = new byte[LightControl.ChannelCount];
        }

        public byte[] CurrentValues { get; }
        public Dictionary<int, byte> getLight()
        {
            var dict = new Dictionary<int, byte>();
            foreach (var pair in lights)
                dict.Add(pair.Key, (byte)(pair.Value * Value));
            return dict;
        }
        public string Name { get => name; set { name = value; RaisePropertyChanged("Name"); } }
        public bool IsOn { get { return Value > 0; } set { Value = value ? 1 : 0; } }
        public int Percentage { get { return (int)(Value * 100); } set { Value = ((double)value)/100; RaisePropertyChanged("Percentage"); } }
        public double Value { get => value; set { this.value = value; RaisePropertyChanged("Value"); RaisePropertyChanged("Percentage"); RaisePropertyChanged("IsOn"); setValues();  } }
        public bool Explicit { get; set; }
        private void setValues()
        {
            foreach(var l in lights)
                CurrentValues[l.Key] = (byte)(l.Value * Value);
            Context.RefreshLights();
        }

        public async Task FadeTo(int milliseconds, double targetValue)
        {
            var diff = targetValue - value;
            if (diff == 0)
                return;
            var tickDiff = diff / milliseconds * LightControl.lightTick;
            while (Math.Abs((value - targetValue)) >= Math.Abs(tickDiff))
            {
                await Context.uiDispatcher.InvokeAsync(() => Value += tickDiff);
                await Task.Delay(LightControl.lightTick);
            }
            await Context.uiDispatcher.InvokeAsync(() => Value = targetValue);
        }
    }
}
