using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class BeginCombatStep : GameStep
    {
        public BeginCombatStep(Context Context) : base(Context, StepType.BeginCombat)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();
            Context.Logic.CombatDamagePhase = 0;

            Context.Logic.attackers.Clear();
            Context.Logic.blockers.Clear();

            Context.Logic.attacker = ap;

            // 507.1. First, if the game being played is a multiplayer game in which the active player’s opponents don’t all automatically become defending players,
            // the active player chooses one of his or her opponents. That player becomes the defending player. This turn-based action doesn’t use the stack. (See rule 506.2.)
            if (Context.Players.Count == 2)
                Context.Logic.defender = Context.Logic.GetNextPlayer(ap);
            else
            {
                Context.Logic.defender = ap.request.RequestFromObjects(RequestType.SelectDefender,
                    $"{ap}: Select player to attack.", Context.Players.Where(x => x != ap));
            }

            // 507.2. Second, any abilities that trigger at the beginning of combat go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            Context.Methods.TriggerEvents(new EventInfoCollection.BeginningOfCombatPhase(ap));

            // 507.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            Context.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}