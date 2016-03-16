using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver;
using Delver.GameSteps;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class DearlyDeparted : Creature
    {
        public DearlyDeparted() : base("4WW", 5, 5)
        {
            Base.Name = "Dearly Departed";
            Base.Subtype.Add("Spirit");
            Base.AddKeyword(Keywords.Flying);

            Base.When(
                $"As long as {this} is in your graveyard, each Human creature you control enters the battlefield with an additional +1/+1 counter on it",
                EventCollection.CreatureEnterTheBattlefield(null, Zone.Graveyard),
                AddCounterToCreature
            );
        }

        public void AddCounterToCreature(BaseEventInfo e)
        {
            if (e.triggerCard.Current.Subtype.Contains("Human") && this.Zone == Zone.Graveyard)
                e.Game.Methods.AddCounter(e.triggerCard, new PlussCounter());
        }
    }
}
