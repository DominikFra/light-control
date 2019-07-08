using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Light
{
    public class LightGroup : PercentageViewModel
    {
        private IEnumerable<LightValue> template;

        public LightGroup(Context context, string name, IEnumerable<LightValue> template) : base(context, name)
        {
            this.template = template;
            CurrentValues = new byte[LightControl.ChannelCount];
        }

        public byte[] CurrentValues { get; }
        public IEnumerable<LightValue> Template
        {
            get { return template; }
            set
            {
                template = value;
                RaisePropertyChanged("Template");
            }
        }
        public Dictionary<int, byte> getLight()
        {
            var dict = new Dictionary<int, byte>();
            foreach (var set in template)
                dict.Add(set.Index - 1, (byte)(set.Value * Value));
            return dict;
        }
        public bool Explicit { get; set; }
        override protected void setValues()
        {
            foreach(var l in template)
                CurrentValues[l.Index -1] = (byte)(l.Value * Value);
            Context.RefreshLights?.Invoke();
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
        public LightGroupModel ToModel()
        {
            return new LightGroupModel()
            {
                Name = Name,
                Template = Template.Select(x => new LightValueModel()
                {
                    Index = x.Index,
                    Value = x.Value
                })
            };
        }
    }
}
