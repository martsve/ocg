﻿using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class PreMainPhase : GameStep
    {
        public PreMainPhase(Game game) : base(game, StepType.PreMain)
        {
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();

            // 505.4. Second, any abilities that trigger at the beginning of the main phase go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            game.Methods.TriggerEvents(new EventInfo.BeginningOfMainStep(game, ap));

            // 505.5. Third, the active player gets priority. Players may cast spells and activate abilities. The active player may play a land.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }
    }
}