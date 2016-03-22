//FalkenrathNoble
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class FalkenrathNoble : Creature 
    {
        public FalkenrathNoble() : base("3B", 2, 2)
        {
            Name = "Falkenrath Noble";
            Base.Subtype.Add("Vampire");
            Base.AddKeyword(Keywords.Flying);

            Base.When(
                $"Whenever Falkenrath Noble or another creature dies, target player loses 1 life and you gain 1 life.",
                EventCollection.CreatureDies(),
                loseAndGainLifeEffect,
                new Target.Player()
            );
        }

        public void loseAndGainLifeEffect(EventInfo e)
        {
            foreach (Player target in e.Targets)
            {
                e.Context.Methods.LoseLife(target, this, 1);
                e.Context.Methods.GainLife(e.SourcePlayer, this, 1);
            }
        }
    }
}
