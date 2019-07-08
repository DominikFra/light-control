using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    public abstract class PercentageViewModel : ContextViewModelBase
    {
        protected string name;
        protected double value;

        public PercentageViewModel(Context context, string name) : base(context)
        {
            this.name = name;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public bool IsOn { get { return Value > 0; } set { Value = value ? 1 : 0; } }
        public int Percentage { get { return (int)(Value * 100); } set { Value = ((double)value) / 100; RaisePropertyChanged("Percentage"); } }
        public double Value { get => value; set { this.value = value; RaisePropertyChanged("Value"); RaisePropertyChanged("Percentage"); RaisePropertyChanged("IsOn"); setValues(); } }
        protected abstract void setValues();
    }
}
