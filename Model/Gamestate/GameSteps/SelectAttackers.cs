using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class SelectAttackers : GameStep
    {
        public SelectAttackers(Game game) : base(game, StepType.SelectAttackers)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            game.SaveState();

            var ap = game.Logic.GetActivePlayer();
            var dp = game.Logic.defender;

            var attackers = new List<Card>();

            while (true)
            {
                // 506.3. Only a creature can attack or block. Only a player or a planeswalker can be attacked.
                attackers.Clear();

                // clear all marks of who creatures are attacking
                foreach (var c in ap.Battlefield)
                {
                    c.IsAttacking = null;
                    c.IsBlocked = false;
                }

                while (true)
                {
                    // 508.1a The active player chooses which creatures that he or she controls, if any, will attack. 
                    // The chosen creatures must be untapped, and each one must either have haste or have been controlled by the active player continuously since the turn began.
                    var list =
                        ap.Battlefield.Where(
                            c =>
                                c.isCardType(CardType.Creature) && c.IsAttacking == null && !c.IsTapped &&
                                (!c.SummonSickness || c.Has(Keywords.Haste)));

                    if (list.Count() == 0)
                        break;

                    var attacker = ap.request.RequestFromObjects(RequestType.SelectAttacker, $"{ap}, Select attacker",
                        list);

                    if (attacker == null)
                        break;

                    // 508.1b If the defending player controls any planeswalkers, or the game allows the active player to attack multiple other players, 
                    // the active player announces which player or planeswalker each of the chosen creatures is attacking.

                    // Select which object it attacks
                    attacker.IsAttacking = game.Methods.SelectObjectToAttack(attacker);
                    attackers.Add(attacker);
                }

                if (attackers.Count > 0)
                {
                    var result = ap.request.RequestYesNo(RequestType.ConfirmAttack, $"{ap}: 1. Complete attack 2. Undo");
                    if (result.Type == InteractionType.Pass)
                        continue;
                }


                // 508.1c The active player checks each creature he or she controls to see whether it’s affected by any restrictions 
                // (effects that say a creature can’t attack, or that it can’t attack unless some condition is met).
                // If any restrictions are being disobeyed, the declaration of attackers is illegal.
                var legalAttacks = true;
                if (!legalAttacks)
                    continue;

                // 508.1d The active player checks each creature he or she controls to see whether it’s affected by any requirements 
                // (effects that say a creature must attack, or that it must attack if some condition is met). 
                // If the number of requirements that are being obeyed is fewer than the maximum possible number of requirements 
                // that could be obeyed without disobeying any restrictions, the declaration of attackers is illegal. 
                // If a creature can’t attack unless a player pays a cost, that player is not required to pay that cost,
                // even if attacking with that creature would increase the number of requirements being obeyed.
                var obeyedRequreiments = 0;
                var maxPossibleObeyed = 0;
                if (obeyedRequreiments < maxPossibleObeyed)
                    continue;

                break;
            }

            // 508.1e If any of the chosen creatures have banding or a “bands with other” ability, the active player announces which creatures, if any, are banded with which. (See rule 702.21, “Banding.”)
            foreach (var card in attackers.Where(c => c.Has(Keywords.Banding)))
            {
                throw new NotImplementedException();
            }

            // 508.1f The active player taps the chosen creatures. Tapping a creature when it’s declared as an attacker isn’t a cost; attacking simply causes creatures to become tapped.
            foreach (var card in attackers.Where(c => !c.Has(Keywords.Vigilance)))
            {
                game.Methods.Tap(card);
            }

            // 508.1g If any of the chosen creatures require paying costs to attack, the active player determines the total cost to attack.
            // Costs may include paying mana, tapping permanents, sacrificing permanents, discarding cards, and so on. Once the total cost is determined, it becomes “locked in.” If effects would change the total cost after this time, ignore this change.
            var attackCost = new ManaCost();

            // 508.1h If any of the costs require mana, the active player then has a chance to activate mana abilities (see rule 605, “Mana Abilities”).
            // 508.1i Once the player has enough mana in his or her mana pool, he or she pays all costs in any order. Partial payments are not allowed.
            var success = game.Logic.TryToPay(ap, attackCost, null);
            if (!success)
            {
                // dirty fail - we should gracefully allow the user to try multiple times (Undo)
                game.PostData("Attacking failed. Reverting!");
                game.RevertState();
            }

            // 508.1j Each chosen creature still controlled by the active player becomes an attacking creature. It remains an attacking creature until it’s removed from combat or the combat phase ends, whichever comes first. See rule 506.4.
            game.Logic.attackers = attackers;

            foreach (var c in attackers)
            {
                game.PostData($"{c} is attacking {c.IsAttacking}");
            }

            // 508.2. Second, any abilities that triggered on attackers being declared go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            if (attackers.Count > 0)
            {
                game.Methods.CollectEvents();
                foreach (var c in attackers)
                    game.Methods.TriggerEvents(new EventInfoCollection.CreatureAttacks(ap, dp, c));
                game.Methods.TriggerEvents(new EventInfoCollection.AttackersDeclared(ap, dp, attackers));
                game.Methods.ReleaseEvents();
            }

            // 508.3. Third, the active player gets priority. Players may cast spells and activate abilities.

            game.CleanState();
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();

            // 508.6. If no creatures are declared as attackers or put onto the battlefield attacking, skip the declare blockers and combat damage steps.
            if (game.Logic.attackers.Count == 0)
            {
                var n = game.CurrentTurn.steps[0];
                while (n.type == StepType.SelectBlockers || n.type == StepType.CombatDamage)
                {
                    game.CurrentTurn.steps.Pop();
                    n = game.CurrentTurn.steps[0];
                }
            }
        }
    }
}