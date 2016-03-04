﻿using System;
using System.Collections.Generic;

namespace Delver.GameSteps
{
    [Serializable]
    internal class EndCombatStep : GameStep
    {
        public EndCombatStep(Game game) : base(game, StepType.EndOfCombat)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();

            // 511.1. First, all “at end of combat” abilities trigger and go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            game.Methods.TriggerEvents(new EventInfo.EndOfCombatStep(game, ap));

            // 511.2. Second, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();

            game.Logic.defender = null;

            // clear all marks of who creatures are attacking
            foreach (var c in game.Logic.attackers)
            {
                c.IsAttacking = null;
                c.IsBlocked = false;
                c.DamageAssignmentOrder = new List<Card>();
            }

            // clear all marks of who creatures are blocking
            foreach (var c in game.Logic.blockers)
            {
                c.IsBlocking = new List<Card>();
                c.DamageAssignmentOrder = new List<Card>();
            }

            game.Logic.attackers = new List<Card>();
            game.Logic.blockers = new List<Card>();
        }
    }
}