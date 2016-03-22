//SlayeroftheWicked
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class SlayeroftheWicked : Creature 
    {
        public SlayeroftheWicked() : base("3W", 3, 2)
        {
            Name = "Slayer of the Wicked";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Soldier");

            Base.When(
                 $"When Slayer of the Wicked enters the battlefield, you may destroy target Vampire, Werewolf, or Zombie.",
                 EventCollection.ThisEnterTheBattlefield(),
                 destroyBadGuy,
                 new Target.Creature(x => isBadGuy(x as Card))
             );
        }

        public bool isBadGuy(Card c) {
            if (c.IsSubType("Vampire") || c.IsSubType("Werewolf") || c.IsSubType("Zombie"))
                return true;
            return false;
        }

        public void destroyBadGuy(EventInfo e)
        {
            foreach (Card target in e.Targets)
                e.Context.Methods.Destroy(e.SourceCard, target);
        }
    }
}
