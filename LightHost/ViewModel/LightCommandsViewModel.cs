using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace LightHost.ViewModel
{
    public class LightCommandsViewModel
    {
        public LightCommandsViewModel(Context context)
        {
            LightGroups = new List<LightGroup>();
            Black = new CommandHandler((obj) => { LightGroups.Where(x => x != BehindStage).Select(x => x.Value = 0).ToArray(); });

            LightGroups.Add(BehindStage = new LightGroup(context, "Hinter der Bühne", context.behindStage));
            LightGroups.Add(Music = new LightGroup(context, "Musik", context.music));
            LightGroups.Add(StageFront = new LightGroup(context, "Bühne Front", context.stageFront));

            LightGroups.Add(Street = new LightGroup(context, "Straße", context.street));
            LightGroups.Add(StreetFront = new LightGroup(context, "Straße Gegenlicht", context.streetFront));
            LightGroups.Add(DoorRight = new LightGroup(context, "Türe", context.doorRight));
            LightGroups.Add(Street2 = new LightGroup(context, "Straße2", context.street2));
            LightGroups.Add(CosmeStairs = new LightGroup(context, "Treppe Cosme", context.cosmeStairs));
            LightGroups.Add(BreakMod = new LightGroup(context, "Umbau", context.breakLight));

            LightGroups.Add(RoomLeft = new LightGroup(context, "Raum links", context.roomLeft));
            LightGroups.Add(RoomLeftFront = new LightGroup(context, "Raum Links Front", context.roomLeftFront));

            LightGroups.Add(RoomRight = new LightGroup(context, "Raum rechts", context.roomRight));
            LightGroups.Add(GapLight = new LightGroup(context, "Spaltlicht", context.gapLight));
            LightGroups.Add(EntranceLight = new LightGroup(context, "Eingang Vitrine", context.entranceLight));
            LightGroups.Add(Spot = new LightGroup(context, "Spot", context.spot));
            LightGroups.Add(Blue = new LightGroup(context, "Blau", context.blue));
        }
        
        public ICommand Black { get; private set; }

        public List<LightGroup> LightGroups { get; }
        public LightGroup Street { get; }
        public LightGroup StreetFront { get; }
        public LightGroup RoomRight { get; }
        public LightGroup RoomLeft { get; }
        public LightGroup StageFront { get; }
        public LightGroup Music { get; }
        public LightGroup DoorRight { get; }
        public LightGroup BehindStage { get; }
        public LightGroup EntranceLight { get; }
        public LightGroup Blue { get; }
        public LightGroup Spot { get; }
        public LightGroup Street2 { get; }
        public LightGroup RoomLeftFront { get; }
        public LightGroup GapLight { get; }
        public LightGroup CosmeStairs { get; }
        public LightGroup BreakMod { get; }
    }
}