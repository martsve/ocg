using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver.GameSteps
{
    [Serializable]
    internal class UntapStep : GameStep
    {
        public UntapStep(Context Context) : base(Context, StepType.Untap)
        {
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();

            // 502.1. First, all phased-in permanents with phasing that the active player controls phase out,
            // and all phased-out permanents that the active player controlled when they phased out phase in. 
            // This all happens simultaneously. This turn-based action doesn’t use the stack. See rule 702.25, “Phasing.”
            foreach (var card in ap.Battlefield.Where(x => x.Has(Keywords.Phasing)))
                throw new NotImplementedException();

            // 502.2. Second, the active player determines which permanents he or she controls will untap. 
            // Then he or she untaps them all simultaneously. This turn-based action doesn’t use the stack.
            // Normally, all of a player’s permanents untap, but effects can keep one or more of a player’s permanents from untapping.
            foreach (var card in ap.Battlefield.Where(x => x.IsTapped))
                if (!card.Marks.ContainsKey(Marks.CANT_UNTAP))
                    Context.Methods.Untap(card);

            // mark all permanents in play
            foreach (var card in ap.Battlefield)
                card.UntapController = ap;

            // 502.3. No player receives priority during the untap step, so no spells can be cast or resolve and no abilities can be activated or resolve.
            // Any ability that triggers during this step will be held until the next time a player would receive priority, 
            // which is usually during the upkeep step. (See rule 503, “Upkeep Step.”)
            var nextStep = Context.CurrentTurn.steps[0];
            nextStep.stack = Context.CurrentStep.stack;
            Context.CurrentStep.stack.Clear();
        }

        public override void Interact()
        {
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}