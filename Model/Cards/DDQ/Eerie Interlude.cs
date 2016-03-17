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
    internal class EerieInterlude : Instant
    {
        public EerieInterlude() : base("2W")
        {
            Name = "Eerie Interlude";
            Base.Effect(
                "Exile any number of target creatures you control. Return those cards to the battlefield under their owner's control at the beginning of the next end step.",
                new FlickerAnyNumberOfOwnCreatures()
            );
        }
    }

    [Serializable]
    internal class FlickerAnyNumberOfOwnCreatures : Effect
    {
        public FlickerAnyNumberOfOwnCreatures()
        {
            SetMultipleTargets(new Target.Permanent(CardType.Creature));
        }

        public override void Invoke(EventInfo e) 
        {
            foreach (Card card in e.Targets)
                e.Game.Methods.Exile(card.Referance);

            // we record new exile-Refs (they get new id after exile)
            exileRefs = e.Targets.ToReferance();

            e.AddDelayedTrigger(
                 $"Return those cards to the battlefield under their owner's control at the beginning of the next end step.",
                 EventCollection.BeginningOfEndStep(),
                 ReturnCreatures
            );
        }

        private ReferanceList<GameObject> exileRefs;

        public void ReturnCreatures(EventInfo e)
        {
            foreach (Card card in exileRefs)
                e.Game.Methods.ChangeZone(card, card.Zone, Zone.Battlefield);
        }
        
    }
}
