using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHost.ViewModel
{
    public class LightChangeViewModel : ChangeViewModel
    {
        public LightChangeViewModel(Context context, LightGroup light, double value = 1, int fadeTime = 0, int delay = 0)
            : base(context, value, fadeTime, delay)
        {
            this.LightGroup = light;
        }
        private LightGroup LightGroup { get; }
        public override string Name { get { return LightGroup != null ? LightGroup.Name : "Black"; } }

        public override async Task setActive()
        {
            await base.setActive();
            if (LightGroup == null)
            {
                await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.Black.Execute(null));
                return;
            }
            await Task.Delay(Delay);
            if (FadeTime == 0)
                await Context.uiDispatcher.InvokeAsync(() => LightGroup.Value = TargetValue);
            else
                await LightGroup.FadeTo(FadeTime, TargetValue);
            await Context.uiDispatcher.InvokeAsync(() => IsRunning = false );
        }
    }
}
