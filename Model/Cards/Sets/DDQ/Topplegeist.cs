//Topplegeist
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class Topplegeist : Creature 
    {
        public Topplegeist() : base("W", 1, 1)
        {
            Name = "Topplegeist";
            Base.Subtype.Add("Spirit");
            Base.AddKeyword(Keywords.Flying);
            
            Base.When(
                 $"When Topplegeist enters the battlefield, tap target creature an opponent controls.",
                 EventCollection.ThisEnterTheBattlefield(),
                 tapCreature,
                 new Target.PermanentOpponentControls(CardType.Creature)
             );

            NotImplemented();

            Base.When(
                 $"Delirium - At the beginning of each opponent's upkeep, if there are four or more card types among cards in your graveyard, tap target creature that player controls.",
                 EventCollection.BeginningOfUpkeep(e => e.Context.ActivePlayer != e.SourcePlayer),
                 tapCreatureIfDelirium //, new Target.PermanentOpponentControls(CardType.Creature, activePlayer?? )
             );
        }

        public void tapCreatureIfDelirium(EventInfo e)
        {
            if (e.SourcePlayer.HasDelirium())
                tapCreature(e);
        }

        public void tapCreature(EventInfo e)
        {
            foreach (Card target in e.Targets)
            {
                e.Context.Methods.Tap(target);
            }
        }

    }
}
