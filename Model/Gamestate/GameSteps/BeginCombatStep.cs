using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class BeginCombatStep : GameStep
    {
        public BeginCombatStep(Game game) : base(game, StepType.BeginCombat)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();
            game.Logic.CombatDamagePhase = 0;

            game.Logic.attackers = new List<Card>();
            game.Logic.blockers = new List<Card>();

            // 507.1. First, if the game being played is a multiplayer game in which the active player’s opponents don’t all automatically become defending players,
            // the active player chooses one of his or her opponents. That player becomes the defending player. This turn-based action doesn’t use the stack. (See rule 506.2.)
            if (game.Players.Count == 2)
                game.Logic.defender = game.Logic.GetNextPlayer(ap);
            else
            {
                game.Logic.defender = ap.request.RequestFromObjects(RequestType.SelectDefender,
                    $"{ap}: Select player to attack.", game.Players.Where(x => x != ap));
            }

            // 507.2. Second, any abilities that trigger at the beginning of combat go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            game.Methods.TriggerEvents(new EventInfo.BeginningOfCombatPhase(game, ap));

            // 507.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }
    }
}