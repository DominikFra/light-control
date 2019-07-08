using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHost.ViewModel
{
    public abstract class ChangeViewModel : ContextViewModelBase
    {
        public ChangeViewModel(Context context, double value, int fadeTime, int delay) : base(context)
        {
            TargetValue = value;
            FadeTime = fadeTime;
            Delay = delay;
        }

        public virtual async Task setActive()
        {
            await Context.uiDispatcher.InvokeAsync(() => this.IsStarted = this.IsRunning = true);
        }
        public abstract string Name { get; }

        private bool isStarted;
        private bool isRunning;

        public bool IsStarted { get => isStarted; set { isStarted = value; RaisePropertyChanged("IsStarted"); } }
        public bool IsRunning { get => isRunning; set { isRunning = value; RaisePropertyChanged("IsRunning"); } }
        public double TargetValue { get; set; }
        public int FadeTime { get; set; }
        public int Delay { get; set; }
    }
}
