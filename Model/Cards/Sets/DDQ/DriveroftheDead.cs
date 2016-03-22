//DriveroftheDead
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class DriveroftheDead : Creature 
    {
        public DriveroftheDead() : base("3B", 3, 2)
        {
            Name = "Driver of the Dead";
            Base.Subtype.Add("Vampire");

            Base.When(
                $"When Driver of the Dead dies, return target creature card with converted mana cost 2 or less from your graveyard to the battlefield.",
                EventCollection.ThisDies(),
                returnTargetCard,
                new Target.CreatureCard(filter: x => (x as Card).Current.CastingCost.Count <= 2, zone: Zone.Graveyard)
            );
        }

        public void returnTargetCard(EventInfo e)
        {
            foreach (Card target in e.Targets)
            {
                 e.Context.Methods.ChangeZone(target, target.Zone, Zone.Battlefield);
            }
        }
    }
}
