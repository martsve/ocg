using System;
using System.Collections.Generic;

namespace Delver.GameSteps
{
    [Serializable]
    internal class EndCombatStep : GameStep
    {
        public EndCombatStep(Context Context) : base(Context, StepType.EndOfCombat)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();

            // 511.1. First, all “at end of combat” abilities trigger and go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            Context.Methods.TriggerEvents(new EventInfoCollection.EndOfCombatStep(ap));

            // 511.2. Second, the active player gets priority. Players may cast spells and activate abilities.
            Context.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();

            Context.Logic.defender = null;

            // clear all marks of who creatures are attacking
            foreach (var c in Context.Logic.attackers)
            {
                c.IsAttacking = null;
                c.IsBlocked = false;
                c.DamageAssignmentOrder.Clear();
            }

            // clear all marks of who creatures are blocking
            foreach (var c in Context.Logic.blockers)
            {
                c.IsBlocking.Clear();
                c.DamageAssignmentOrder.Clear();
            }

            Context.Logic.attackers.Clear();
            Context.Logic.blockers.Clear();
        }
    }
}