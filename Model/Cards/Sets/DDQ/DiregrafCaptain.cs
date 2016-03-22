//DiregrafCaptain
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class DiregrafCaptain : Creature 
    {
        public DiregrafCaptain() : base("1UB", 2, 2)
        {
            Name = "Diregraf Captain";
            Base.Subtype.Add("Zombie");
            Base.Subtype.Add("Soldier");
            Base.AddKeyword(Keywords.Deathtouch);
            Base.Text = @"Other Zombie creatures you control get +1/+1. ";

            Base.When(
                 $"Whenever another Zombie you control dies, target opponent loses 1 life.",
                 EventCollection.CreatureDies(x => x.TriggerCard.IsSubType("Zombie")),
                 e => e.Context.Methods.LoseLife(e.Targets.First() as Player, this, 1),
                 new Target.Opponent()
            );

            NotImplemented();
        }
    }
}
