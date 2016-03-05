using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class DearlyDeparted : Creature
    {
        public DearlyDeparted() : base("4WW", 5, 5)
        {
            Name = "Dearly Departed";
            Subtype.Add("Spirit");
            AddKeyword(Keywords.Flying);

            Events.Add(new Events.CreatureEnterTheBattlefield(CreatureEnter, Zone.Graveyard)
            {
                Text = $"As long as {this} is in your graveyard, each Human creature you control enters the battlefield with an additional +1/+1 counter on it"
            });
        }

        public void CreatureEnter(BaseEventInfo e)
        {
            if (e.triggerCard.Subtype.Contains("Human") && this.Zone == Zone.Graveyard)
                e.Game.Methods.AddCounter(e.triggerCard, new PlussCounter());
        }
    }
}
