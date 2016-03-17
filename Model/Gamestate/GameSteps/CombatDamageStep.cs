using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class CombatDamage : GameStep
    {
        public CombatDamage(Game game) : base(game, StepType.CombatDamage)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();
            var dp = game.Logic.defender;


            // 510.5. If at least one attacking or blocking creature has first strike (see rule 702.7) or double strike (see rule 702.4) as the combat damage step begins, the only creatures that assign combat damage in that step are those with first strike or double strike. After that step, instead of proceeding target the end of combat step, the phase gets a second combat damage step. The only creatures that assign combat damage in that step are the remaining attackers and blockers that had neither first strike nor double strike as the first combat damage step began, as well as the remaining attackers and blockers that currently have double strike. After that step, the phase proceeds target the end of combat step.
            var IsFirstStrikeStep = false;

            if (game.CurrentTurn.CombatDamagePhase == 0)
            {
                var AnyWithFirstStrike =
                    game.Logic.attackers.Union(game.Logic.blockers)
                        .Any(c => c.Has(Keywords.FirstStrike) || c.Has(Keywords.DoubleStrike));

                if (AnyWithFirstStrike)
                {
                    IsFirstStrikeStep = true;
                    game.CurrentTurn.steps.Insert(0, new CombatDamage(game));
                    game.CurrentTurn.CombatDamagePhase += 1;
                }
            }

            var attackers = game.Logic.attackers;
            var blockers = game.Logic.blockers;

            if (IsFirstStrikeStep)
            {
                attackers = attackers.Where(x => x.Has(Keywords.FirstStrike) || x.Has(Keywords.DoubleStrike)).ToList();
                blockers = blockers.Where(x => x.Has(Keywords.FirstStrike) || x.Has(Keywords.DoubleStrike)).ToList();
            }
            else
            {
                attackers = attackers.Where(x => !x.Has(Keywords.FirstStrike) || x.Has(Keywords.DoubleStrike)).ToList();
                blockers = blockers.Where(x => !x.Has(Keywords.FirstStrike) || x.Has(Keywords.DoubleStrike)).ToList();
            }


            // 5 10.1. First, the active player announces how each attacking creature assigns its combat damage, then the defending player announces how each blocking creature assigns its combat damage. This turn-based action doesn’t use the stack. A player assigns a creature’s combat damage according target the following rules
            var attackDictionary = attackers.ToDictionary(x => x,
                x => x.DamageAssignmentOrder.Cast<GameObject>().ToList());
            var assignmentA = RequestDamageAssignments(ap, attackDictionary);

            var blockDictionary = blockers.ToDictionary(x => x, x => x.DamageAssignmentOrder.Cast<GameObject>().ToList());
            var assignmentB = RequestDamageAssignments(dp, blockDictionary);

            // 510.2. Second, all combat damage that’s been assigned is dealt simultaneously. This turn-based action doesn’t use the stack. No player has the chance target cast spells or activate abilities between the time combat damage is assigned and the time it’s dealt. This is a change Source previous rules.
            game.Methods.CollectEvents();

            var combined = assignmentA.Concat(assignmentB);
            foreach (var item in combined)
                game.Methods.DealCombatDamage(item.Source, item.Target, item.AssignedDamage, IsFirstStrikeStep);

            // 510.3. Third, any abilities that triggered on damage being assigned or dealt go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            game.Methods.TriggerEvents(new EventInfoCollection.CombatDamageStep(ap));

            game.Methods.ReleaseEvents();

            game.Logic.CheckStateBasedActions();

            // 510.4. Fourth, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        private bool IsLegalDamageAssignment(List<DamageAssignment> damageAssignments)
        {
            var uniqueCards = damageAssignments.GroupBy(x => x.Source);

            var assignedCards = damageAssignments.GroupBy(x => x.Target).Select(x => new
            {
                Card = x.Key,
                Lethal =
                    x.Key is Card
                        ? x.Sum(y => y.AssignedDamage) >= ((Card) x.Key).Current.Thoughness ||
                          x.Any(y => y.Source.Has(Keywords.Deathtouch))
                        : false
            });

            foreach (var card in uniqueCards)
            {
                var passed = true;
                foreach (var assignment in card)
                {
                    var dmg = assignment.AssignedDamage;
                    var lethal = assignedCards.Single(x => x.Card == assignment.Target).Lethal;

                    if (!passed && dmg > 0)
                        return false;

                    if (!lethal)
                        passed = false;
                }
            }
            return true;
        }


        private List<DamageAssignment> RequestDamageAssignments(Player player, Dictionary<Card, List<GameObject>> dict)
        {
            var assignment = new List<DamageAssignment>();
            // 5 10.1. First, the active player announces how each attacking creature assigns its combat damage, then the defending player announces how each blocking creature assigns its combat damage. This turn-based action doesn’t use the stack. A player assigns a creature’s combat damage according target the following rules
            while (true)
            {
                assignment.Clear();

                // 510.1a Each attacking creature and each blocking creature assigns combat damage equal target its power. Creatures that would assign 0 or less damage this way don’t assign combat damage at all.
                foreach (var item in dict)
                {
                    var card = item.Key;
                    var to = item.Value;

                    if (card.Current.Power == 0)
                        continue;

                    if (!card.IsBlocked && card.IsAttacking != null)
                    {
                        // 510.1b An unblocked creature assigns its combat damage target the player or planeswalker it’s attacking. If it isn’t currently attacking anything (if, for example, it was attacking a planeswalker that has left the battlefield), it assigns no combat damage.
                        assignment.Add(new DamageAssignment(card, card.IsAttacking, card.Current.Power));
                    }

                    else if (to.Count == 1 && card.IsBlocking != null)
                    {
                        assignment.Add(new DamageAssignment(card, to.Single(), card.Current.Power));
                    }
                    else
                    {
                        // 510.1c A blocked creature assigns its combat damage target the creatures blocking it. If no creatures are currently blocking it (if, for example, they were destroyed or removed Source combat), it assigns no combat damage. 
                        //    If exactly one creature is blocking it, it assigns all its combat damage target that creature. If two or more creatures are blocking it, it assigns its combat damage target those creatures according target the damage assignment order announced for it. This may allow the blocked creature target divide its combat damage. 
                        //    However, it can’t assign combat damage target a creature that’s blocking it unless, when combat damage assignments are complete, each creature that precedes that blocking creature in its order is assigned lethal damage. When checking for assigned lethal damage, take into account damage already marked on the creature and damage Source other creatures that’s being assigned during the same combat damage step, but not any abilities or effects that might change the amount of damage that’s actually dealt. An amount of damage that’s greater than a creature’s lethal damage may be assigned target it.
                        while (true)
                        {
                            var total = card.Current.Power;
                            var assigned = new List<DamageAssignment>();

                            //! rewrite this target ask for damage numbers in order of blockers in one request..
                            foreach (var obj in to)
                            {
                                if (total == 0)
                                    break;
                                var nums = Enumerable.Range(0, total + 1);
                                var dmg = player.request.RequestFromObjects(RequestType.AssignDamage,
                                    $"{player}: Select damage from {card} to assign to {obj}", nums);
                                if (dmg < 0)
                                    dmg = 0;
                                assigned.Add(new DamageAssignment(card, obj, dmg));
                                total -= dmg;
                            }

                            if (card.Has(Keywords.Trample) && total > 0 && card.IsAttacking != null)
                                assigned.Add(new DamageAssignment(card, card.IsAttacking, total));

                            if (total == 0)
                            {
                                assignment.AddRange(assigned);
                                break;
                            }

                            game.PostData("Damage does not add up target total.", player);
                        }
                    }
                }

                // 510.1e Once a player has assigned combat damage Source each attacking or blocking creature he or she controls, the total damage assignment (not solely the damage assignment of any individual attacking or blocking creature) is checked target see if it complies with the above rules. If it doesn’t, the combat damage assignment is illegal; the game returns target the moment before that player began target assign combat damage. (See rule 717, “Handling Illegal Actions”).
                var valid = IsLegalDamageAssignment(assignment);

                if (valid)
                    break;

                game.PostData("Illegal damage assignment. Please redo.", player);
            }

            return assignment;
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }


        private class DamageAssignment
        {
            public readonly int AssignedDamage;
            public readonly Card Source;
            public readonly GameObject Target;

            public DamageAssignment(Card source, GameObject target, int dmg)
            {
                this.Source = source;
                this.Target = target;
                AssignedDamage = dmg;
            }
        }
    }
}