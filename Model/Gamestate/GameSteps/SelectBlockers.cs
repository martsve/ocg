using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver.GameSteps
{
    [Serializable]
    internal class SelectBlockers : GameStep
    {
        public SelectBlockers(Context Context) : base(Context, StepType.SelectBlockers)
        {
            IsCombatStep = true;
        }

        public override void Enter()
        {
            Context.SaveState();

            var ap = Context.Logic.GetActivePlayer();
            var dp = Context.Logic.defender;
            Context.Logic.blockers = new List<Card>();


            var blockers = new List<Card>();

            while (true)
            {
                blockers.Clear();
                // clear all marks of who creatures are attacking
                foreach (var c in dp.Battlefield)
                    c.IsBlocking.Clear();

                // 509.1. First, the defending player declares blockers. This turn-based action doesn’t use the stack. 
                // To declare blockers, the defending player follows the steps below, in order. 
                // If at any point during the declaration of blockers, the defending player is unable to comply with any of the steps listed below,
                // the declaration is illegal; the game returns to the moment before the declaration (see rule 717, “Handling Illegal Actions”).

                while (true)
                {
                    // 509.1a The defending player chooses which creatures that he or she controls, if any, will block. The chosen creatures must be untapped.
                    // For each of the chosen creatures, the defending player chooses one creature for it to block that’s attacking him, her, or a planeswalker he or she controls.
                    var list =
                        dp.Battlefield.Where(c => c.isCardType(CardType.Creature) && c.IsBlocking.Count == 0 && !c.IsTapped);

                    if (!list.Any())
                        break;

                    var blocker = dp.request.RequestFromObjects(MessageType.SelectBlocker, $"{dp}: Select blocker", list);

                    if (blocker == null)
                        break;

                    Card blocking;
                    if (Context.Logic.attackers.Count() > 1)
                    {
                        blocking = dp.request.RequestFromObjects(MessageType.SelectAttackerToBlock,
                            $"{dp}: Select creature for {blocker} to block", Context.Logic.attackers);
                        if (blocking == null)
                            continue;
                    }
                    else
                        blocking = Context.Logic.attackers.First();

                    blocker.IsBlocking.Add(blocking);
                    blockers.Add(blocker);
                }


                if (blockers.Count > 0)
                {
                    var result = dp.request.RequestYesNo(MessageType.ConfirmBlock, $"{dp}: 1. Complete block 2. Undo");
                    if (result.Type == InteractionType.Pass)
                        continue;
                }

                // 509.1b The defending player checks each creature he or she controls to see whether it’s affected by any restrictions 
                // (effects that say a creature can’t block, or that it can’t block unless some condition is met). If any restrictions are being disobeyed, 
                // the declaration of blockers is illegal.
                // A restriction may be created by an evasion ability (a static ability an attacking creature has that restricts what can block it).
                // If an attacking creature gains or loses an evasion ability after a legal block has been declared, it doesn’t affect that block. 
                // Different evasion abilities are cumulative.
                var legalBlocks = true;
                if (!legalBlocks)
                    continue;

                // 509.1c The defending player checks each creature he or she controls to see whether it’s affected by any requirements 
                // (effects that say a creature must block, or that it must block if some condition is met). If the number of requirements that are being obeyed is fewer 
                // than the maximum possible number of requirements that could be obeyed without disobeying any restrictions, the declaration of blockers is illegal. 
                // If a creature can’t block unless a player pays a cost, that player is not required to pay that cost, even if blocking with that creature would 
                // increase the number of requirements being obeyed.
                var obeyedRequreiments = 0;
                var maxPossibleObeyed = 0;
                if (obeyedRequreiments < maxPossibleObeyed)
                    continue;

                break;
            }

            // 509.1d If any of the chosen creatures require paying costs to block, the defending player determines the total cost to block. Costs may include paying mana, tapping permanents, sacrificing permanents, discarding cards, and so on. Once the total cost is determined, it becomes “locked in.” If effects would change the total cost after this time, ignore this change.
            var blockCost = new ManaCost();

            // 509.1e If any of the costs require mana, the defending player then has a chance to activate mana abilities (see rule 605, “Mana Abilities”).
            var success = Context.Logic.TryToPay(dp, blockCost, null);
            if (!success)
            {
                // dirty fail - we should gracefully allow the user to try multiple times (Undo)
                MessageBuilder.Error("Blocking failed. Reverting!").To(dp).Send(Context);
                Context.RevertState();
            }

            // 509.1f Once the player has enough mana in his or her mana pool, he or she pays all costs in any order. Partial payments are not allowed.


            // 509.1g Each chosen creature still controlled by the defending player becomes a blocking creature. Each one is blocking the attacking creatures chosen for it. It remains a blocking creature until it’s removed from combat or the combat phase ends, whichever comes first. See rule 506.4.

            // 509.1h An attacking creature with one or more creatures declared as blockers for it becomes a blocked creature; one with no creatures declared as blockers for it becomes an unblocked creature. This remains unchanged until the creature is removed from combat, an effect says that it becomes blocked or unblocked, or the combat phase ends, whichever comes first. A creature remains blocked even if all the creatures blocking it are removed from combat.
            foreach (var blocker in blockers)
                foreach (var c in blocker.IsBlocking)
                    c.IsBlocked = true;

            // 509.2. Second, for each attacking creature that’s become blocked, the active player announces that creature’s damage assignment order, which consists of the creatures blocking it in an order of that player’s choice. (During the combat damage step, an attacking creature can’t assign combat damage to a creature that’s blocking it unless each creature ahead of that blocking creature in its order is assigned lethal damage.) This turn-based action doesn’t use the stack.
            foreach (var attacker in Context.Logic.attackers)
            {
                var blockersForAtttacker = blockers.Where(x => x.IsBlocking.Contains(attacker));
                if (blockersForAtttacker.Count() > 1)
                    attacker.DamageAssignmentOrder = ap.request.RequestMultiple(attacker, MessageType.OrderAttackers,
                        $"Order {attacker}’s damage assignment order (First is damaged first):", blockersForAtttacker,
                        true);
                else
                    attacker.DamageAssignmentOrder = blockersForAtttacker.ToList();
            }

            // 509.2a During the declare blockers step, if a blocking creature is removed from combat or a spell or ability causes it to stop blocking an attacking creature, the blocking creature is removed from all relevant damage assignment orders. The relative order among the remaining blocking creatures is unchanged.

            // 509.3. Third, for each blocking creature, the defending player announces that creature’s damage assignment order, which consists of the creatures it’s blocking in an order of that player’s choice. (During the combat damage step, a blocking creature can’t assign combat damage to a creature it’s blocking unless each creature ahead of that blocked creature in its order is assigned lethal damage.) This turn-based action doesn’t use the stack.
            foreach (var blocker in blockers)
            {
                var attackersForblocker = blocker.IsBlocking;
                if (attackersForblocker.Count() > 1)
                    blocker.DamageAssignmentOrder = dp.request.RequestMultiple(blocker, MessageType.OrderBlockers,
                        $"Order {blocker}’s damage assignment order (First is damaged first):", attackersForblocker,
                        true);
                else
                    blocker.DamageAssignmentOrder = attackersForblocker.ToList();
            }

            // 509.3a During the declare blockers step, if an attacking creature is removed from combat or a spell or ability causes it to stop being blocked by a blocking creature, the attacking creature is removed from all relevant damage assignment orders. The relative order among the remaining attacking creatures is unchanged.


            Context.Logic.blockers = blockers;

            foreach (var c in blockers)
                MessageBuilder.SetBlocking(c, c.DamageAssignmentOrder).Send(Context);


            // 509.4. Fourth, any abilities that triggered on blockers being declared go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            if (blockers.Count > 0)
            {
                Context.Methods.CollectEvents();
                foreach (var c in blockers)
                    Context.Methods.TriggerEvents(new EventInfoCollection.CreatuerBlocks(ap, dp, c, c.IsBlocking));

                Context.Methods.TriggerEvents(new EventInfoCollection.BlockersDeclared(ap, dp, blockers));
                Context.Methods.ReleaseEvents();
            }


            // 509.5. Fifth, the active player gets priority. Players may cast spells and activate abilities.

            // 509.6. If a spell or ability causes a creature on the battlefield to block an attacking creature, the active player announces the blocking creature’s placement in the attacking creature’s damage assignment order. The relative order among the remaining blocking creatures is unchanged. Then the defending player announces the attacking creature’s placement in the blocking creature’s damage assignment order. The relative order among the remaining attacking creatures is unchanged. This is done as part of the blocking effect.

            // 509.7. If a creature is put onto the battlefield blocking, its Controller chooses which attacking creature it’s blocking as it enters the battlefield (unless the effect that put it onto the battlefield specifies what it’s blocking), then the active player announces the new creature’s placement in the blocked creature’s damage assignment order. The relative order among the remaining blocking creatures is unchanged. A creature put onto the battlefield this way is “blocking” but, for the purposes of trigger events and effects, it never “blocked.”

            Context.CleanState();
            Context.Logic.SetWaitingPriorityList();
        }


        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}