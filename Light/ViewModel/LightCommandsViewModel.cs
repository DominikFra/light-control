using Light.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

namespace Light
{
    public class LightCommandsViewModel : ContextViewModelBase
    {
        private LightControl Light;

        public LightCommandsViewModel() : base(new Context())
        {
            Context.RefreshLights = RefreshLights;
            Light = new LightControl();
            LightGroups = new ObservableCollection<LightGroup>();
            LightEffects = new ObservableCollection<EffectViewModel>();

            Black = new CommandHandler((obj) => { LightGroups.Select(x => x.Value = 0).ToArray(); });
            Configure = new CommandHandler(ShowConfiguration);
            AddLightGroupCommand = new CommandHandler(AddLightGroup);
            DeleteLightGroupCommand = new CommandHandler(DeleteLightGroup);
            AddLightEffectCommand = new CommandHandler(AddLightEffect);
            DeleteLightEffectCommand = new CommandHandler(DeleteLightEffect);
            ShowTemplateCommand = new CommandHandler(ShowTemplate);
            TakeAsTemplateCommand = new CommandHandler(TakeAsTemplate);
            
            LoadConfigurationFromJson();
        }

        public ICommand Black { get; private set; }
        public ICommand Configure { get; private set; }
        public ICommand AddLightGroupCommand { get; private set; }
        public ICommand DeleteLightGroupCommand { get; private set; }
        public ICommand AddLightEffectCommand { get; private set; }
        public ICommand DeleteLightEffectCommand { get; private set; }
        public ICommand ShowTemplateCommand { get; private set; }
        public ICommand TakeAsTemplateCommand { get; private set; }

        public ObservableCollection<LightGroup> LightGroups { get; set; }
        public ObservableCollection<EffectViewModel> LightEffects { get; set; }

        private LightGroup selectedLightGroup;
        public LightGroup SelectedLightGroup
        {
            get => selectedLightGroup;
            set
            {
                selectedLightGroup = value;
                RaisePropertyChanged("SelectedLightGroup");
            }
        }
        private EffectViewModel selectedLightEffect;
        public EffectViewModel SelectedLightEffect
        {
            get => selectedLightEffect;
            set
            {
                selectedLightEffect = value;
                RaisePropertyChanged("SelectedLightEffect");
            }
        }
        public IEnumerable<LightValue> CurrentLight
        {
            get
            {
                return Light.Current()
                    .Select((value, index) => new LightValue(index + 1, value, Light));
            }
        }

        public bool Changed { get; set; }

        internal void RefreshLights()
        {
            Context.uiDispatcher.Invoke(() =>
            {
                var values = LightGroups.Select(x => x.CurrentValues).ToArray();
                var finalValues = new byte[LightControl.ChannelCount];
                for (int i = 0; i < LightControl.ChannelCount; i++)
                    finalValues[i] = values.Select(x => x[i]).Max();

                var explicits = LightGroups.Where(x => x.Explicit);
                foreach (var x in explicits)
                {
                    var lights = x.getLight();
                    foreach (var y in lights)
                        finalValues[y.Key] = y.Value;
                }
                for (int i = 0; i < LightControl.ChannelCount; i++)
                    Light.SetChannelValue(i, finalValues[i]);
                RaisePropertyChanged("CurrentLight");
            });
        }

        public void OnClosing()
        {
            SaveLightConfig();
        }

        internal void RaiseCurrentLightChanged()
        {
            RaisePropertyChanged("CurrentLight");
        }


        private void AddLightGroup(object obj)
        {
            var newLightGroup = new LightGroup(Context, "UNBENANNT", null);
            LightGroups.Add(newLightGroup);
            SelectedLightGroup = newLightGroup;
            TakeAsTemplate(null);
            Changed = true;
        }

        private void AddLightEffect(object obj)
        {
            var newEffect = new EffectViewModel(Context, "UNBENANNT");
            LightEffects.Add(newEffect);
            SelectedLightEffect = newEffect;
            Changed = true;
        }
        private void ShowConfiguration(object obj)
        {
            new Window()
            {
                Content = new ConfigurationView { DataContext = this },
                Title = "Light Configuration"
            }
            .ShowDialog();
        }
        private void DeleteLightGroup(object obj)
        {
            LightGroups.Remove(SelectedLightGroup);
            SelectedLightGroup = LightGroups.FirstOrDefault();
            Changed = true;
        }
        private void DeleteLightEffect(object obj)
        {
            LightEffects.Remove(SelectedLightEffect);
            SelectedLightEffect = LightEffects.FirstOrDefault();
            Changed = true;
        }

        private void TakeAsTemplate(object obj)
        {
            SelectedLightGroup.Template = CurrentLight.Where(x => x.Value > 0).ToArray();
            Changed = true;
        }

        private void ShowTemplate(object obj)
        {
            this.Black.Execute(null);
            foreach (var channel in SelectedLightGroup.Template)
                this.Light.SetChannelValue(channel.Index - 1, channel.Value);
            RaisePropertyChanged("CurrentLight");
        }

        private void FromModel(ConfigModel model)
        {
            foreach (var group in model.LightGroups)
            {
                var template = group.Template.Select(x => new LightValue(x.Index, x.Value, Light));
                LightGroups.Add(new LightGroup(Context, group.Name, template));
            }
            foreach(var effect in model.LightEffects)
            {
                var vm = new EffectViewModel(Context, effect.Name);
                foreach (var change in effect.Changes)
                {
                    var changeVm = new LightChangeViewModel(Context, LightGroups.FirstOrDefault(g => g.Name == change.Name), (x) => vm.DeleteChange(x), change.Value, change.FadeTime, change.Delay);
                    vm.Changes.Add(changeVm);
                }
                LightEffects.Add(vm);
            }
        }
        private string lightConfigPath;
        private void SaveLightConfig()
        {
            if (Changed)
            {
                var result = MessageBox.Show("Es gab Änderungen am Licht. Soll gespeichert werden?", "Speichern", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var dlg = new SaveFileDialog();
                    dlg.FileName = lightConfigPath;
                    dlg.DefaultExt = ".lightconfig";
                    dlg.Filter = "Show definition (.lightconfig)|*.lightconfig";
                    dlg.AddExtension = true;
                    var hasFile = dlg.ShowDialog();
                    if (hasFile == true)
                    {
                        var obj = new ConfigModel()
                        {
                            LightGroups = LightGroups.Select(x => x.ToModel()).ToList(),
                            LightEffects = LightEffects.Select(x => x.ToModel()).ToList()
                        };
                        var text = JsonConvert.SerializeObject(obj);
                        File.WriteAllText(dlg.FileName, text);
                    }
                }
            }
        }

        private void LoadConfigurationFromJson()
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".lightconfig";
            dlg.Filter = "Show definition (.lightconfig)|*.lightconfig";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                this.lightConfigPath = dlg.FileName;
                var content = File.ReadAllText(dlg.FileName);
                var model = JsonConvert.DeserializeObject<ConfigModel>(content);
                FromModel(model);
            }
        }
    }
}