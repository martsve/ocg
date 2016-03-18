using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;
using Delver;

//namespace Delver.Cards.DDQ

namespace Delver.Cards
{
    [Serializable]
    internal class ElderCathar : Creature
    {
        public ElderCathar() : base("2W", 2, 2)
        {
            Name = "Elder Cathar";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Soldier");

            Base.When (
                $"When {this} dies, put a +1/+1 counter on target creature you control. If that creature is a Human, put two +1/+1 counters on it instead.",
                EventCollection.ThisDies(),
                PutCounterOn,
                new Target.PermanentYouControl(CardType.Creature)
            );
        }

        public void PutCounterOn(EventInfo e)
        {
            foreach (Card card in e.Targets)
            {
                e.Context.Methods.AddCounter(card, new PlussCounter());
                if (card.Current.Subtype.Contains("Human"))
                    e.Context.Methods.AddCounter(card, new PlussCounter());
            }
        }
    }
}
