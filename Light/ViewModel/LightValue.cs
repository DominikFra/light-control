using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    public class LightValue: ViewModelBase
    {
        private byte value;
        private LightControl light;

        public LightValue(int index, byte value, LightControl light)
        {
            this.Index = index;
            this.value = value;
            this.light = light;
        }
        public int Index { get; }
        public byte Value
        {
            get { return value; }
            set
            {
                this.value = value;
                light.SetChannelValue(Index - 1, value);
                RaisePropertyChanged("Value");
            }
        }
    }
}
