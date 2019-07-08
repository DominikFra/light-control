using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Light
{
    public class EffectViewModel : ContextViewModelBase
    {
        protected string name;

        public EffectViewModel(Context context, string name) : base(context)
        {
            this.name = name;
            Changes = new ObservableCollection<LightChangeViewModel>();
            AddChangeCommand = new CommandHandler(x => AddChange());
            ExecuteCommand = new CommandHandler(async x => await Execute());
        }

        public ICommand ExecuteCommand { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public ObservableCollection<LightChangeViewModel> Changes { get; set; }
        public async Task Execute()
        {
            await Task.WhenAll(Changes.Select(x => x.setActive()).ToList());
        }
        public ICommand AddChangeCommand { get; private set; }
        private void AddChange()
        {
            var change = new LightChangeViewModel(Context, null, DeleteChange);
            Changes.Add(change);
        }

        public void DeleteChange(LightChangeViewModel change)
        {
            Changes.Remove(change);
        }

        internal EffectModel ToModel()
        {
            return new EffectModel()
            {
                Name = Name,
                Changes = Changes.Select(x => new ChangeModel()
                {
                    Name = x.Name,
                    Delay = x.Delay,
                    FadeTime = x.FadeTime,
                    Value = x.TargetValue
                }).ToList()
            };
        }
    }
}
