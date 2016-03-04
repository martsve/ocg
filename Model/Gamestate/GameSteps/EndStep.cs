﻿using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class EndStep : GameStep
    {
        public EndStep(Game game) : base(game, StepType.End)
        {
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();


            // 513.1. First, all abilities that trigger “at the beginning of the end step” or “at the beginning of the next end step” go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            game.Methods.TriggerEvents(new EventInfo.BeginningOfEndStep(game, ap));

            // 513.2. Second, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();

            // 513.3. If a permanent with an ability that triggers “at the beginning of the end step” enters the battlefield during this step, that ability won’t trigger 
            // until the next turn’s end step. Likewise, if a delayed triggered ability that triggers “at the beginning of the next end step” is created during this step, 
            // that ability won’t trigger until the next turn’s end step. In other words, the step doesn’t “back up” so those abilities can go on the stack.
            // This rule applies only to triggered abilities; it doesn’t apply to continuous effects whose durations say “until end of turn” or “this turn.” (See rule 514, “Cleanup Step.”)
        }
    }
}