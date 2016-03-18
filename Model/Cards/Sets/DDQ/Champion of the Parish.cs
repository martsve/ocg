using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver;

//namespace Delver.Cards.DDQ
namespace Delver.Cards
{
    [Serializable]
    internal class ChampionoftheParish : Creature
    {
        public ChampionoftheParish() : base("W", 1, 1)
        {
            Name = "Champion of the Parish";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Cleric");

            Base.When(
                 $"Whenever another Human enters the battlefield under your control, put a +1/+1 counter on {this}.",
                 EventCollection.CreatureEnterTheBattlefield(filter),
                 PutCounterOnCreature
            );
        }

        public bool filter(EventInfo e)
        {
            return e.TriggerCard != e.SourceCard && e.TriggerCard.Current.Subtype.Contains("Human");
        }

        public void PutCounterOnCreature(EventInfo e)
        {
            if (filter(e))
                e.Context.Methods.AddCounter(this, new PlussCounter());
        }
    }
}
