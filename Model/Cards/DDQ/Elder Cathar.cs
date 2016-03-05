using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;
using Delver.Effects;

//namespace Delver.Cards.DDQ

namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class ElderCathar : Creature
    {
        public ElderCathar() : base("2W", 2, 2)
        {
            Name = "Elder Cathar";
            Subtype.Add("Human");
            Subtype.Add("Soldier");

            Events.Add(
                new Events.ThisDies(
                    new TargetedCallbackEffect(ThisDies,
                    new List<ITarget>() { new Target.PermanentYouControl(CardType.Creature) }
                )
                ) {
                    Text = $"When {this} dies, put a +1/+1 counter on target creature you control. If that creature is a Human, put two +1/+1 counters on it instead."
                }
            );
        }
        public void ThisDies(BaseEventInfo e, List<ITarget> targets)
        {
            foreach (Card card in targets)
            {
                e.Game.Methods.AddCounter(card, new PlussCounter());
                if (card.Subtype.Contains("Human"))
                    e.Game.Methods.AddCounter(card, new PlussCounter());
            }
        }
    }
}
