using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Light
{
    public class LightChangeViewModel : ChangeViewModel
    {
        public LightChangeViewModel(Context context, LightGroup light, Action<LightChangeViewModel> delete, double value = 1, int fadeTime = 0, int delay = 0)
            : base(context, value, fadeTime, delay)
        {
            this.LightGroup = light;
            DeleteChangeCommand = new CommandHandler(x => delete(this));
        }
        public LightGroup LightGroup { get; set; }
        public override string Name { get { return LightGroup != null ? LightGroup.Name : "Black"; } }

        public override async Task setActive()
        {
            await base.setActive();
            await Task.Delay(Delay);
            if (FadeTime == 0)
                await Context.uiDispatcher.InvokeAsync(() => LightGroup.Value = TargetValue);
            else
                await LightGroup.FadeTo(FadeTime, TargetValue);
            await Context.uiDispatcher.InvokeAsync(() => IsRunning = false );
        }
        public ICommand DeleteChangeCommand { get; private set; }
    }
}
