using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace LightHost.ViewModel
{
    public class ProgramViewModel : ContextViewModelBase
    {
        public ICommand NextCommand { get; private set; }
        public ICommand ActivateStepCommand { get; private set; }
        public ICommand ApplauseCommand { get; private set; }
        public StepViewModel ActiveStep { get => activeStep; set { activeStep = value; RaisePropertyChanged("ActiveStep"); } }
        public StepViewModel SelectedStep { get => selectedStep; set { selectedStep = value; RaisePropertyChanged("SelectedStep"); } }
        public ObservableCollection<StepViewModel> Steps { get; }
        private ChangeViewModel applause;

        public ProgramViewModel() : base(new Context())
        {
            applause = new EffectChangeViewModel(Context);
            NextCommand = new CommandHandler(async x => await activateNextStep());
            ActivateStepCommand = new CommandHandler(async x => await activateSelectedStep());
            ApplauseCommand = new CommandHandler(async x => await applause.setActive());
            Steps = new ObservableCollection<StepViewModel>();
            addStep("", "Start", new LightChangeViewModel(Context, Context.LightCommands.BehindStage));
            addStep("",  "Musiklicht", new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 1000)
                , new SoundChangeViewModel(Context, Context.intro, 1, 2000));
            addStep("", "Ansage", new LightChangeViewModel(Context, Context.LightCommands.DoorRight)
                , new SoundChangeViewModel(Context, Context.intro, 0.05, 2000, 0));
            addStep("", "Ansage Ende", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0, 1000)
                , new SoundChangeViewModel(Context, Context.intro, 1, 2000, 0));
            addStep("", "Vorhang geht auf", new LightChangeViewModel(Context, Context.LightCommands.StageFront, 1, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 1, 2000, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 1, 2000, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 3000, 2000)
                , new SoundChangeViewModel(Context, Context.intro, 0.05, 1000, 2000)
                , new SoundChangeViewModel(Context, Context.intro, 0, 2000, 3000));
            addStep("S5", "Schock-Auftritt Tobi", new LightChangeViewModel(Context, Context.LightCommands.Street)
                , new LightChangeViewModel(Context, Context.LightCommands.StreetFront));
            addStep("S7", "Wenn Nick auf der Bühne ist", new LightChangeViewModel(Context, Context.LightCommands.StreetFront, 0,1000));
            addStep("S7", "Tobi wirft seinen Hut, Auftritt Tür rechts", new LightChangeViewModel(Context, Context.LightCommands.DoorRight));
            addStep("S8", "Kampf geht los", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0, 2000));
            addStep("S9", "\"Ach wie schön... Lorbeer...\"", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 1, 3990));
            addStep("S12", "Abgang & Musik", new LightChangeViewModel(Context, Context.LightCommands.StageFront, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.Street, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 1000, 1000));
            addStep("S12", "Umbau zuende & Auftritt links (Musik Fade out)", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 1, 3000));
            addStep("S12", "Musik aus", new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 2000));
            addStep("S18", "Abgang => Rechtes Zimmer", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue, 1, 500));
            addStep("S18", "Auftritt mit Kerze", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.7)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000, 10000));
            addStep("S21", "Abgang rechts", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0, 80));
            addStep("S21", "Auftritt durch Eingang", new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0.7, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0, 2000, 2000));
            addStep("S24", "Abgang durch Eingang", new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0.7, 800)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0, 2000, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0, 800, 1200));
            addStep("S24", "Auftritt von Rechts", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000, 5000));
            addStep("S25", "Auftritt 2. Kerze", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.7, 500));
            addStep("S30", "Abgang Laterne", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.4, 500));
            addStep("S30", "Kerze auspusten => Musik", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 3000, 1000));
            addStep("S31", "Zimmer links", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 1, 1000));
            addStep("S31", "Musik aus", new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 2000));
            addStep("S36", "Abgang und nach Vorne", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.StageFront, 1, 1000, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 1, 0, 1000));
            addStep("S37", "Rodrigo kommt in die Mitte", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0, 2000));
            addStep("S37", "Straßenauftritt", new LightChangeViewModel(Context, Context.LightCommands.StreetFront));
            addStep("S37", "Gegenlicht aus", new LightChangeViewModel(Context, Context.LightCommands.StreetFront, 0, 3000));
            addStep("S39", "Tür rechts", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 1, 1000));
            addStep("S39", "Tür rechts aus", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0, 2000));
            addStep("S39", "Abgang / Rechtes Zimmer", new LightChangeViewModel(Context, Context.LightCommands.StageFront, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue, 1, 1000, 1000));
            addStep("S39", "Kurz danach: Tobi mit Kerze", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000, 10000));
            addStep("S40", "Cosme ab mit Kerze", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S41", "Cosme kommt wieder rein", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000, 5000));
            addStep("S41", "Kerze auspusten", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S42", "Cosme kommt wieder mit Licht", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5));
            addStep("S45", "Abgang / Linkes Zimmer", new LightChangeViewModel(Context, null)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0.8, 1000, 2000));
            addStep("S46", "Tür rechts", new LightChangeViewModel(Context, Context.LightCommands.DoorRight)
                , new LightChangeViewModel(Context, Context.LightCommands.StageFront, 0.5, 1000));
            addStep("S47", "Tür rechts aus", new LightChangeViewModel(Context, Context.LightCommands.DoorRight, 0, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.StageFront, 0, 2000));
            addStep("S49", "Abgang", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 2000, 1000)
                , new SoundChangeViewModel(Context, Context.night, 1, 2000, 1000));
            addStep("S49", "Musik fade out & Straße", new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 2000)
                , new SoundChangeViewModel(Context, Context.night, 0.05, 2000)
                , new SoundChangeViewModel(Context, Context.night, 0, 3000, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.Street2, 1, 2000, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.StreetFront, 0.8, 2000, 1000));
            addStep("S50", "Wenn Cosme vorne, Gegenlicht aus, Spaltlicht vorbereiten"
                , new LightChangeViewModel(Context, Context.LightCommands.StreetFront, 0, 3000)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000));
            addStep("S50", "Abgang & Rechter Raum", new LightChangeViewModel(Context, Context.LightCommands.Street2, 0)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue));
            addStep("S51", "Zwischendurch Spaltlicht aus", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 4000));
            addStep("S51", "Claudia macht die Kerze an", new LightChangeViewModel(Context, Context.LightCommands.Spot, 1)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5, 5000));
            addStep("S55", "Kerze wird aufgenommen", new LightChangeViewModel(Context, Context.LightCommands.Spot, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000));
            addStep("S55", "Manuel und Cosme ab", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S55", "Manuel und Cosme kommen wieder", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000, 4000));
            addStep("S56", "Manuel mit Laterne in Alkoven", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S56", "Manuel kommt wieder", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5));
            addStep("S57", "Zimmer rechts fade out & SAALLICHT!", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0,2000)
                , new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 2000, 1000)
                , new SoundChangeViewModel(Context, Context.breakStart, 1, 2000, 1000));
            addStep("S57", "Wenn Vorhang zu", new LightChangeViewModel(Context, Context.LightCommands.BreakMod, 1, 5000));
            addStep("S57", "Musiklicht aus", new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 1000));
            addStep("S58", "Ende der Pause: Musik & SAALLICHT AUS", new LightChangeViewModel(Context, Context.LightCommands.Music, 1, 2000)
                , new SoundChangeViewModel(Context, Context.breakEnd, 1, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.BreakMod, 0, 5000));
            addStep("S58", "Linkes Zimmer von Vorne", new LightChangeViewModel(Context, Context.LightCommands.RoomLeftFront, 1, 3000));
            addStep("S58", "Nach Auftritt Manuel & Isabel", new LightChangeViewModel(Context, Context.LightCommands.Music, 0, 2000, 2000)
                , new SoundChangeViewModel(Context, Context.breakEnd, 0.03, 2000, 2000)
                , new SoundChangeViewModel(Context, Context.breakEnd, 0, 1000, 4000));
            addStep("S58", "Linkes Zimmer \"jetzt wappne dich\"", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 1, 3000, 0));
            addStep("S62", "Manuel und Isabel durch Wandschrank, dann;", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000));
            addStep("S62", "Abgang, anderes Zimmer", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeftFront, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue, 1, 1000, 1000));
            addStep("S62", "Wenn Isabel den Raum verlassen hat", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000));
            addStep("S64", "Vorbereitung Spaltlicht", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000));
            addStep("S65", "Wenn Isabel mit Cosme raus", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000));
            addStep("S66", "Abgang Manuel => Linkes Zimmer", new LightChangeViewModel(Context, Context.LightCommands.Blue, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0.8, 1000, 2000));
            addStep("S67", "Cosme geht nach Vorne und setzt sich", new LightChangeViewModel(Context, Context.LightCommands.CosmeStairs, 1, 1000));
            addStep("S67", "Cosme steht wieder auf", new LightChangeViewModel(Context, Context.LightCommands.CosmeStairs, 0, 1000));
            addStep("S70", "Wenn Luis abgegangen ist, Spaltlicht", new LightChangeViewModel(Context, Context.LightCommands.GapLight, 1, 5000));
            addStep("S70", "Abgang links", new LightChangeViewModel(Context, Context.LightCommands.RoomLeft, 0, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.Blue, 1, 1000, 1000));
            addStep("S70", "Spaltaufgang Luis mit licht", new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0.7, 1000)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.EntranceLight, 0, 2000, 2000)
                , new LightChangeViewModel(Context, Context.LightCommands.GapLight, 0, 5000, 10000));
            addStep("S74", "aus für Tobi abgang", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S74", "an für juan", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5));
            addStep("S74", "Abgang Juan => aus", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0));
            addStep("S74", "an für Tobi", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 0.5));
            addStep("S78", "langsam heller", new LightChangeViewModel(Context, Context.LightCommands.RoomRight, 1, 5000));
            addStep("S79", "Black", new LightChangeViewModel(Context, null));
            addStep("S79", "Applaus", new LightChangeViewModel(Context, Context.LightCommands.StageFront)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft));
            addStep("S79", "Black", new LightChangeViewModel(Context, null));
            addStep("S79", "Applaus", new LightChangeViewModel(Context, Context.LightCommands.StageFront)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft));
            addStep("S79", "Black", new LightChangeViewModel(Context, null));
            addStep("S79", "Applaus", new LightChangeViewModel(Context, Context.LightCommands.StageFront)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomRight)
                , new LightChangeViewModel(Context, Context.LightCommands.RoomLeft));

            selectStep(0);
            this.ActivateStepCommand.Execute(null);
        }
        
        private void addStep(string page, string name, params ChangeViewModel[] changes)
        {
            Steps.Add(new StepViewModel(page, name, Context, changes));
        }

        private async Task activateSelectedStep()
        {
            await deactivateCurrentStep();
            await Context.uiDispatcher.InvokeAsync(() => Context.LightCommands.Black.Execute(null));
            await activateStep(selectedStep);
        }

        private async Task activateNextStep()
        {
            var activeStep = await deactivateCurrentStep();
            if (activeStep == null)
                return;
            var i = Steps.IndexOf(activeStep);
            if ((i + 1) >= Steps.Count)
                return;
            await activateStep(Steps[i + 1]);
        }

        private async Task<StepViewModel> deactivateCurrentStep()
        {
            var activeStep = Steps.FirstOrDefault(x => x.IsActive);
            if (activeStep != null)
                await activeStep.setActive(false);
            return activeStep;
        }
        private async Task activateStep(StepViewModel step)
        {
            if (step == null)
                return;
            ActiveStep = step;
            await Context.uiDispatcher.InvokeAsync(() => 
                selectStep(Steps.IndexOf(step) + 1));

            if (step.IsActive)
                await step.setActive(false);
            await step.setActive(true);
        }
        private void selectStep(int index)
        {
            if (Steps.Count > (index))
                SelectedStep = Steps[index];
        }

        private StepViewModel selectedStep;
        private StepViewModel activeStep;
    }
}
