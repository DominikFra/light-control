using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LightHost.ViewModel
{
    public class SoundChangeViewModel : ChangeViewModel
    {
        MediaPlayer music;
        public SoundChangeViewModel(Context context, MediaPlayer music, double value = 1, int fadeTime = 0, int delay = 0) : base(context, value, fadeTime, delay)
        {
            this.music = music;
            music.Volume = 0;
        }
        public override string Name { get { return "Musik"; } }

        bool started;
        public override async Task setActive()
        {
            await base.setActive();
            await Task.WhenAll(FadeVolume(), Play());
        }
        private async Task Play()
        {
            if (!started)
            {
                music.Play();
                started = true;
                await Task.Delay((int)music.NaturalDuration.TimeSpan.TotalMilliseconds);
            }
        }
        private async Task FadeVolume()
        {
            await Task.Delay(Delay);
            var diff = TargetValue - music.Volume;
            if (diff == 0)
                return;
            var tickDiff = diff / FadeTime * LightControl.lightTick;
            while (Math.Abs((music.Volume - TargetValue)) >= Math.Abs(tickDiff))
            {
                await Context.uiDispatcher.InvokeAsync(() => music.Volume += tickDiff);
                await Task.Delay(LightControl.lightTick);
            }
            await Context.uiDispatcher.InvokeAsync(() => music.Volume = TargetValue);
        }
    }
}
