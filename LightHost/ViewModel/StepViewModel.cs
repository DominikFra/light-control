using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHost.ViewModel
{
    public class StepViewModel : ContextViewModelBase
    {
        private bool isActive;

        public string Page { get; set; }
        public string Name { get; set; }
        public IEnumerable<ChangeViewModel> Changes{ get; }
        public bool IsActive { get => isActive; set { Task.Run(() => setActive(value)); } }

        public async Task setActive(bool value)
        {
            isActive = value;
            await Context.uiDispatcher.InvokeAsync(() => RaisePropertyChanged("IsActive"));
            if (value)
            {
                List<Task> tasks = new List<Task>();
                foreach (var t in Changes)
                    tasks.Add(t.setActive());
                await Task.WhenAll(tasks);
            }
        }

        public StepViewModel(string page, string name, Context context, IEnumerable<ChangeViewModel> changes) : base(context)
        {
            this.Page = page;
            this.Name = name;
            this.Changes = changes;
        }
    }
}
