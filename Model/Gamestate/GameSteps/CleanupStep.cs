﻿using System;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class CleanupStep : GameStep
    {
        public CleanupStep(Game game) : base(game, StepType.Cleanup)
        {
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();

            // 514.1. First, if the active player’s hand contains more cards than his or her maximum hand size (normally seven),
            // he or she discards enough cards to reduce his or her hand size to that number. This turn-based action doesn’t use the stack.
            while (ap.Hand.Count() > ap.HandLimit)
            {
                Card card = null;
                while (card == null)
                {
                    card = ap.request.RequestFromObjects(RequestType.DiscardACard, "Select a card to discard:", ap.Hand);
                }
                game.Methods.Discard(ap, card);
            }

            var e = new EventInfo()
            {
                Game = game,
            };

            // 514.2. Second, the following actions happen simultaneously: all damage marked on permanents (including phased-out permanents)
            // is removed and all “until end of turn” and “this turn” effects end. This turn-based action doesn’t use the stack.
            foreach (var layer in game.LayeredEffects.ToList())
            {
                if (layer.Duration == Duration.EndOfTurn || layer.Duration == Duration.NextCleanup)
                {
                    layer.End(e);
                    game.LayeredEffects.Remove(layer);
                }
            }

            foreach (var p in game.Players)
            {
                foreach (var card in p.Battlefield)
                {
                    card.Damage = 0;
                    card.DeathtouchDamage = false;
                }
            }

            // 514.3a At this point, the game checks to see if any state-based actions would be performed and/or 
            // any triggered abilities are waiting to be put onto the stack (including those that trigger “at the beginning of the next cleanup step”). 
            // If so, those state-based actions are performed, then those triggered abilities are put on the stack, then the active player gets priority.
            // Players may cast spells and activate abilities. Once the stack is empty and all players pass in succession, another cleanup step begins.
            game.Logic.CheckStateBasedActions();

            game.Methods.TriggerEvents(new EventInfoCollection.BeginningOfNextCleanupStep(ap));

            if (stack.Count > 0)
            {
                game.Logic.SetWaitingPriorityList();
                game.CurrentTurn.steps.Insert(0, new CleanupStep(game));
            }
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }
    }
}