using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHost.ViewModel
{
    public class EffectChangeViewModel : ChangeViewModel
    {
        public EffectChangeViewModel(Context context) : base(context, 0, 0, 0)
        {
        }

        public override string Name => "Applaus";

        public override async Task setActive()
        {
            await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.Black.Execute(null));
            await base.setActive();
            await Task.Delay(200);
            await setChannelValue(0, 255);
            await Task.Delay(200);
            await setChannelValue(1, 255);
            await setChannelValue(4, 255);
            await Task.Delay(200);
            await setChannelValue(5, 255);
            await Task.Delay(200);
            await setChannelValue(0, 0);
            await Task.Delay(200);
            await setChannelValue(1, 0);
            await setChannelValue(4, 0);
            await Task.Delay(200);
            await setChannelValue(5, 0);
            await Task.Delay(500);
            await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.Street.Value = 1);
            await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.RoomLeft.Value = 1);
            await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.RoomRight.Value = 1);
        }
        private async Task setChannelValue(int channel, byte value)
        {
            await Context.uiDispatcher.InvokeAsync(() =>
            {
                Context.Light.SetChannelValue(channel, value);
                Context.RaiseCurrentLightChanged();
            });
        }
    }
}
