using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;
using Delver.View;

namespace Delver
{
    [Serializable]
    internal class GameLogic
    {
        private readonly Game game;

        public GameLogic(Game game)
        {
            this.game = game;
        }


        public List<Card> attackers { get; set; }
        public List<Card> blockers { get; set; }
        public Player defender { get; set; }

        public int CombatDamagePhase { get; set; }


        public void DecideStarting()
        {
            var p = GetNextPlayer();
            var action = p.request.RequestYesNo(RequestType.Mulligan, $"Start the game {p}? 1. Yes / 2. No");

            if (action.Type != InteractionType.Accept)
            {
                game.TurnOrder.RemoveAt(0);
                game.TurnOrder.Add(p);
            }
        }

        public void InitializeGame()
        {
            // Initialize game
            SetTurnOrder();
            DecideStarting();
            game.PostData("Turn order: " + string.Join(",", game.TurnOrder));
        }

        public void SetTurnOrder()
        {
            game.TurnOrder = (from p in game.Players
                orderby p.Id descending
                select p).ToList();
        }

        public Player GetNextPlayer(Player current)
        {
            if (game.Players.Count == 0) throw new NoPlayersException("No players added to the game");
            var order = game.TurnOrder;
            var pos = order.FindIndex(p => p == current);
            return order[(pos + 1)%order.Count];
        }

        public Player GetNextPlayer()
        {
            if (game.Players.Count == 0) throw new NoPlayersException("No players added to the game");
            if (game.ActivePlayer == null)
                return game.TurnOrder[0];
            return GetNextPlayer(game.ActivePlayer);
        }

        public Player GetActivePlayer()
        {
            if (game.ActivePlayer == null)
                return GetNextPlayer();
            return game.ActivePlayer;
        }

        public List<Player> GetTurnOrder()
        {
            var list = new List<Player>();
            list.Add(GetActivePlayer());
            for (var i = 0; i < game.Players.Count; i++)
                list.Add(GetNextPlayer(list.Count == 0 ? GetActivePlayer() : list.Last()));
            return list;
        }


        public void Interact()
        {
            var stack = string.Join(", ", game.CurrentStep.stack.Select(x => x.ToString()));
            var ap = game.CurrentStep.PriorityPlayer;

            game.PostData($"Stack: {stack}");

            // 116.3c If a player has priority when he or she casts a spell, activates an ability,
            // or takes a special action, that player receives priority afterward.
            // 116.3d If a player has priority and chooses not to take any actions, that player passes. 
            // If any mana is in that player’s mana pool, he or she announces what mana is there.
            // Then the next player in turn order receives priority.
            Interaction action = null;
            while (action == null)
            {
                var list = new List<InteractionType>
                {
                    InteractionType.Pass,
                    InteractionType.Cast,
                    InteractionType.Ability,
                    InteractionType.GetView,
                    InteractionType.Replay
                };
                var select = ap.request.RequestFromObjects(RequestType.TakeAction, $"{ap}: Perform an action", list);

                if (select == InteractionType.Pass)
                {
                    action = new Interaction(ap, InteractionType.Pass);
                }
                else if (select == InteractionType.Cast)
                {
                    action = new Interaction(ap, InteractionType.Cast);
                    var card = ap.request.RequestFromObjects(RequestType.Cast, $"{ap}: Select card to cast from hand",
                        ap.Hand);
                    action.Card = card;
                    if (card == null)
                        action = null;
                }
                else if (select == InteractionType.Ability)
                {
                    action = new Interaction(ap, InteractionType.Ability);
                    var card = ap.request.RequestFromObjects(RequestType.Activate,
                        $"{ap}: Select card to active ability on", ap.Battlefield.Where(x => x.HasActivatedAbilities()));
                    action.Card = card;
                    if (card == null)
                        action = null;
                }

                else if (select == InteractionType.GetView)
                {
                    var view = GameviewPopulator.GetView(game, ap);
                    game.PostData(view.ToJson());
                }
                else if (select == InteractionType.Replay)
                {
                    action = new Interaction(ap, InteractionType.Pass);
                }
            }

            game.Logic.PerformAction(action);
        }


        public Stack<Player> GetPriorityOrder()
        {
            return GetPriorityOrder(game.Logic.GetActivePlayer());
        }

        public Stack<Player> GetPriorityOrder(Player ap)
        {
            var order = new List<Player>();
            var p = ap;
            do
            {
                order.Add(p);
                p = game.Logic.GetNextPlayer(p);
            } while (p != ap);
            order.Reverse();
            return new Stack<Player>(order);
        }

        public void SetWaitingPriorityList(Player priority_player = null)
        {
            game.CurrentStep.order = GetPriorityOrder(priority_player ?? game.Logic.GetActivePlayer());
        }

        public void SetPriorityPlayer()
        {
            if (game.CurrentStep.order.Count > 0)
            {
                game.CurrentStep.priority.Push(game.CurrentStep.order.Pop());
                game.CurrentStep.PriorityPlayer = game.CurrentStep.priority.Peek();
            }
            else
                game.CurrentStep.PriorityPlayer = null;
        }

        public void ResolveStack()
        {
            // 116.4. If all players pass in succession (that is, if all players pass without taking any actions in between passing), 
            // the spell or ability on top of the stack resolves or, if the stack is empty, the phase or step ends.
            var stack_item = game.CurrentStep.stack.Pop();
            stack_item.Resolve(game);
            game.Logic.CheckStateBasedActions();
            SetWaitingPriorityList();
        }

        public void PerformAction(Interaction action)
        {
            if (action.Type == InteractionType.Pass)
            {
                game.CurrentStep.priority.Pop();
                return;
            }
            if (action.Type == InteractionType.Cast)
            {
                TryToCastCard(action, Zone.Hand);
                game.Logic.SetWaitingPriorityList(action.Player);
            }

            if (action.Type == InteractionType.Ability)
            {
                TryToUseAbility(action.Player, action.Card);
                game.Logic.SetWaitingPriorityList(action.Player);
            }
        }


        public void ApplyLayering()
        {
            foreach (var player in game.Players)
                foreach (var card in player.Battlefield.Where(x => x.isType(CardType.Creature)))
                {
                    card.Power = card.BasePower;
                    card.Thoughness = card.BaseThoughness;
                }

            foreach (var effect in game.LayeredEffects.OrderBy(x => x.Layer).ThenBy(x => x.Timestamp))
            {
                effect.Invoke();
            }
        }

        public void CheckStateBasedActions()
        {
            game.Methods.CollectEvents();

            /*116.5. Each time a player would get priority, the game first performs all applicable state-based actions as a single event 
             * (see rule 704, “State-Based Actions”), then repeats this process until no state-based actions are performed. 
             * Then triggered abilities are put on the stack (see rule 603, “Handling Triggered Abilities”). 
             * These steps repeat in order until no further state-based actions are performed and no abilities trigger. 
             * Then the player who would have received priority does so.
             * 
             * http://mtgsalvation.gamepedia.com/State-based_actions
             */

            ApplyLayering();

            foreach (var p in game.Players)
            {
                if (p.Life <= 0)
                {
                    game.Methods.LoseTheGame(p, $"{p} has {p.Life} life and lost the game.");
                }

                foreach (var c in p.Battlefield.Where(c => c.isType(CardType.Creature)).ToList())
                {
                    if (c.Thoughness == 0)
                        game.Methods.Die(c, Zone.Battlefield);

                    if (!c.Has(Keywords.Indestructible) && c.Damage >= c.Thoughness)
                        game.Methods.Die(c, Zone.Battlefield);
                }
            }

            ApplyLayering();

            game.Methods.ReleaseEvents();
        }

        public void PlayLandCard(Interaction action, Zone from)
        {
            var c = action.Card;

            if (game.CurrentTurn.LandsPlayed > 0)
            {
                game.PostData("Playing land failed: Already played land");
                return;
            }

            game.CurrentTurn.LandsPlayed++;
            game.Methods.ChangeZone(action.Card, from, Zone.Battlefield);
        }


        // http://mtgsalvation.gamepedia.com/Casting_Spells
        public void TryToCastCard(Interaction action, Zone from)
        {
            var c = action.Card;

            if (!c.IsCastable(game))
            {
                game.PostData("Unable to play spell");
                return;
            }

            if (c is Land)
            {
                PlayLandCard(action, from);
            }

            else
            {
                // 601.2. To cast a spell is to take it from where it is (usually the hand), put it on the stack, and pay its costs, so that it will eventually resolve and have its effect. 
                // Casting a spell includes proposal of the spell (rules 601.2a–e) and determination and payment of costs (rules 601.2f–h). To cast a spell, a player follows the steps listed below, in order.
                // If, at any point during the casting of a spell, a player is unable to comply with any of the steps listed below, the casting of the spell is illegal; the game returns to the moment before the casting of that spell was proposed (see rule 717, “Handling Illegal Actions”).
                game.SaveState();

                var success = CastCard(action, from);

                if (!success)
                {
                    game.PostData("Casting failed. Reverting!");
                    game.RevertState();
                }

                game.CleanState();
            }
        }


        public bool CastCard(Interaction action, Zone from)
        {
            var p = action.Player;

            // 601.2a To propose the casting of a spell, a player first moves that card (or that copy of a card) from where it is to the stack. It becomes the topmost object on the stack. It has all the characteristics of the card (or the copy of a card) associated with it, and that player becomes its Controller. The spell remains on the stack until it’s countered, it resolves, or an effect moves it elsewhere. Whether casting the proposed spell is a legal action isn’t checked at this time.
            if (action.Card is IStackCard)
            {
                game.Methods.ChangeZone(action.Card, from, Zone.Stack);
            }
            else
                throw new Exception("Invalid card on stack!");

            return
                PerformCasting(p, (Spell) action.Card);
        }


        public void TryToUseAbility(Player player, Card card, bool onlyManaAbility = false)
        {
            var abilities = card.Abilities
                .Where(a => a.type == AbiltiyType.Activated && (!onlyManaAbility || (onlyManaAbility && a.IsManaSource)))
                .Where(x => x.CanPay(game, player, card))
                .ToList();

            // 602.2. To activate an ability is to put it onto the stack and pay its costs, so that it will eventually resolve and have its effect. Only an object’s Controller (or its owner, if it doesn’t have a Controller) can activate its activated ability unless the object specifically says otherwise. Activating an ability follows the steps listed below, in order. If, at any point during the activation of an ability, a player is unable to comply with any of those steps, the activation is illegal; the game returns to the moment before that ability started to be activated (see rule 717, “Handling Illegal Actions”). Announcements and payments can’t be altered after they’ve been made.

            // 602.2a The player announces that he or she is activating the ability. If an activated ability is being activated from a hidden zone, the card that has that ability is revealed. That ability is created on the stack as an object that’s not a card. It becomes the topmost object on the stack. It has the text of the ability that created it, and no other characteristics. Its Controller is the player who activated the ability. The ability remains on the stack until it’s countered, it resolves, or an effect moves it elsewhere.

            // 602.2b The remainder of the process for activating an ability is identical to the process for casting a spell listed in rules 601.2b–i. Those rules apply to activating an ability just as they apply to casting a spell. An activated ability’s analog to a spell’s mana cost (as referenced in rule 601.2f) is its activation cost.
            if (abilities.Any())
            {
                // only save if we are not casting spells (have previous save)
                if (!onlyManaAbility)
                    game.SaveState();

                var ability = abilities.Count() == 1
                    ? abilities[0]
                    : player.request.RequestFromObjects(RequestType.SelectAbility, $"Select ability to use for {card}:",
                        abilities);

                var success = ability != null && UseAbility(player, card, ability);

                if (!success)
                    game.RevertState();

                game.CleanState();
            }
        }

        public bool UseAbility(Player player, Card card, Ability ability)
        {
            var abilitySpell = new AbilitySpell(game, player, card, ability);

            if (ability.IsManaSource)
            {
                var success = PerformAbility(abilitySpell);
                if (success)
                    abilitySpell.Resolve(game);
                return success;
            }
            game.Methods.AddAbilityToStack(abilitySpell);
            return PerformAbility(abilitySpell);
        }


        public bool PerformAbility(AbilitySpell card)
        {
            // 601.2b If the spell is modal, the player announces the mode choice (see rule 700.2). If the player wishes to splice any cards onto the spell (see rule 702.46), he or she reveals those cards in his or her hand. If the spell has alternative or additional costs that will be paid as it’s being cast such as buyback or kicker costs (see rules 117.8 and 117.9), the player announces his or her intentions to pay any or all of those costs (see rule 601.2f). A player can’t apply two alternative methods of casting or two alternative costs to a single spell. If the spell has a variable cost that will be paid as it’s being cast (such as an {X} in its mana cost; see rule 107.3), the player announces the value of that variable. If a cost that will be paid as the spell is being cast includes hybrid mana symbols, the player announces the nonhybrid equivalent cost he or she intends to pay. If a cost that will be paid as the spell is being cast includes Phyrexian mana symbols, the player announces whether he or she intends to pay 2 life or the corresponding colored mana cost for each of those symbols. Previously made choices (such as choosing to cast a spell with flashback from a graveyard or choosing to cast a creature with morph face down) may restrict the player’s options when making these choices.

            // 601.2c The player announces his or her choice of an appropriate player, object, or zone for each target the spell requires. A spell may require some targets only if an alternative or additional cost (such as a buyback or kicker cost), or a particular mode, was chosen for it; otherwise, the spell is cast as though it did not require those targets. If the spell has a variable number of targets, the player announces how many targets he or she will choose before he or she announces those targets. The same target can’t be chosen multiple times for any one instance of the word “target” on the spell. However, if the spell uses the word “target” in multiple places, the same object, player, or zone can be chosen once for each instance of the word “target” (as long as it fits the targeting criteria). If any effects say that an object or player must be chosen as a target, the player chooses targets so that he or she obeys the maximum possible number of such effects without violating any rules or effects that say that an object or player can’t be chosen as a target. The chosen players, objects, and/or zones each become a target of that spell. (Any abilities that trigger when those players, objects, and/or zones become the target of a spell trigger at this point; they’ll wait to be put on the stack until the spell has finished being cast.)

            // 601.2d If the spell requires the player to divide or distribute an effect (such as damage or counters) among one or more targets, the player announces the division. Each of these targets must receive at least one of whatever is being divided.

            // 601.2e Based on the previous announcements, the game checks to see if the proposed spell can legally be cast based on applicable timing rules (including ones based on the card’s type) and other effects that may allow a spell to be cast or prohibit a spell from being cast. If the proposed spell is illegal, the game returns to the moment before the casting of that spell was proposed (see rule 717, “Handling Illegal Actions”).

            // 601.2f The player determines the total cost of the spell. Usually this is just the mana cost. Some spells have additional or alternative costs. Some effects may increase or reduce the cost to pay, or may provide other alternative costs. Costs may include paying mana, tapping permanents, sacrificing permanents, discarding cards, and so on. The total cost is the mana cost or alternative cost (as determined in rule 601.2b), plus all additional costs and cost increases, and minus all cost reductions. If multiple cost reductions apply, the player may apply them in any order. If the mana component of the total cost is reduced to nothing by cost reduction effects, it is considered to be {0}. It can’t be reduced to less than {0}. Once the total cost is determined, any effects that directly affect the total cost are applied. Then the resulting total cost becomes “locked in.” If effects would change the total cost after this time, they have no effect.
            // 601.2g If the total cost includes a mana payment, the player then has a chance to activate mana abilities (see rule 605, “Mana Abilities”). Mana abilities must be activated before costs are paid.
            // 601.2h The player pays the total cost in any order. Partial payments are not allowed. Unpayable costs can’t be paid.
            var success = true;

            foreach (var ability in card.Abilities)
                foreach (var cost in ability.costs)
                    success = success && cost.TryToPay(game, card.Owner, card.Source);

            // 601.2i Once the steps described in 601.2a–h are completed, the spell becomes cast. Any abilities that trigger when a spell is cast or put onto the stack trigger at this time. If the spell’s Controller had priority before casting it, he or she gets priority.

            // sucess
            return success;
        }


        public bool PerformCasting(Player player, Spell card)
        {
            var success = true;

            // 601.2b If the spell is modal, the player announces the mode choice (see rule 700.2). If the player wishes to splice any cards onto the spell (see rule 702.46), he or she reveals those cards in his or her hand. If the spell has alternative or additional costs that will be paid as it’s being cast such as buyback or kicker costs (see rules 117.8 and 117.9), the player announces his or her intentions to pay any or all of those costs (see rule 601.2f). A player can’t apply two alternative methods of casting or two alternative costs to a single spell. If the spell has a variable cost that will be paid as it’s being cast (such as an {X} in its mana cost; see rule 107.3), the player announces the value of that variable. If a cost that will be paid as the spell is being cast includes hybrid mana symbols, the player announces the nonhybrid equivalent cost he or she intends to pay. If a cost that will be paid as the spell is being cast includes Phyrexian mana symbols, the player announces whether he or she intends to pay 2 life or the corresponding colored mana cost for each of those symbols. Previously made choices (such as choosing to cast a spell with flashback from a graveyard or choosing to cast a creature with morph face down) may restrict the player’s options when making these choices.

            // 601.2c The player announces his or her choice of an appropriate player, object, or zone for each target the spell requires. A spell may require some targets only if an alternative or additional cost (such as a buyback or kicker cost), or a particular mode, was chosen for it; otherwise, the spell is cast as though it did not require those targets. If the spell has a variable number of targets, the player announces how many targets he or she will choose before he or she announces those targets. The same target can’t be chosen multiple times for any one instance of the word “target” on the spell. However, if the spell uses the word “target” in multiple places, the same object, player, or zone can be chosen once for each instance of the word “target” (as long as it fits the targeting criteria). If any effects say that an object or player must be chosen as a target, the player chooses targets so that he or she obeys the maximum possible number of such effects without violating any rules or effects that say that an object or player can’t be chosen as a target. The chosen players, objects, and/or zones each become a target of that spell. (Any abilities that trigger when those players, objects, and/or zones become the target of a spell trigger at this point; they’ll wait to be put on the stack until the spell has finished being cast.)
            foreach (var ability in card.Abilities)
                if (ability.type == AbiltiyType.Effect)
                    success = success && ability.Populate(game, player, card);

            // 601.2d If the spell requires the player to divide or distribute an effect (such as damage or counters) among one or more targets, the player announces the division. Each of these targets must receive at least one of whatever is being divided.

            // 601.2e Based on the previous announcements, the game checks to see if the proposed spell can legally be cast based on applicable timing rules (including ones based on the card’s type) and other effects that may allow a spell to be cast or prohibit a spell from being cast. If the proposed spell is illegal, the game returns to the moment before the casting of that spell was proposed (see rule 717, “Handling Illegal Actions”).

            // 601.2f The player determines the total cost of the spell. Usually this is just the mana cost. Some spells have additional or alternative costs. Some effects may increase or reduce the cost to pay, or may provide other alternative costs. Costs may include paying mana, tapping permanents, sacrificing permanents, discarding cards, and so on. The total cost is the mana cost or alternative cost (as determined in rule 601.2b), plus all additional costs and cost increases, and minus all cost reductions. If multiple cost reductions apply, the player may apply them in any order. If the mana component of the total cost is reduced to nothing by cost reduction effects, it is considered to be {0}. It can’t be reduced to less than {0}. Once the total cost is determined, any effects that directly affect the total cost are applied. Then the resulting total cost becomes “locked in.” If effects would change the total cost after this time, they have no effect.                
            var cost = card.CastingCost;

            // 601.2g If the total cost includes a mana payment, the player then has a chance to activate mana abilities (see rule 605, “Mana Abilities”). Mana abilities must be activated before costs are paid.
            // 601.2h The player pays the total cost in any order. Partial payments are not allowed. Unpayable costs can’t be paid.
            success = success && TryToPay(player, cost, card);

            var usedToPay = player.SelectedFromManaPool;

            // 601.2i Once the steps described in 601.2a–h are completed, the spell becomes cast. Any abilities that trigger when a spell is cast or put onto the stack trigger at this time. If the spell’s Controller had priority before casting it, he or she gets priority.

            // sucess
            return success;
        }


        public bool TryToPay(Player player, ManaCost cost, Card source)
        {
            if (cost.Count == 0)
                return true;

            player.SelectedFromManaPool = new ManaCost();

            while (!player.SelectedFromManaPool.Contains(cost))
            {
                var ok = RequestManaAbility(player, source);
                if (!ok) return false;
            }

            player.SelectedFromManaPool = player.SelectedFromManaPool.UsedToPay(cost);

            player.ManaPool.RemoveExactly(player.SelectedFromManaPool);
            return true;
        }


        public bool RequestManaAbility(Player player, Card source)
        {
            var list = new List<GameObject>();
            foreach (var m in player.ManaPool)
                if (!player.SelectedFromManaPool.ContainsExactly(m))
                    list.Add(m);

            foreach (var c in player.Battlefield.Where(c => c.HasManaSource(game, player, c)))
                list.Add(c);

            var obj = player.request.RequestFromObjects(RequestType.ManaAbility,
                $"{player}, Select mana to pay for {source}", list);
            if (obj != null)
            {
                if (obj is Card)
                {
                    var c = (Card) obj;
                    game.Logic.TryToUseAbility(player, c, true);
                    return true;
                }
                if (obj is Mana)
                {
                    player.SelectedFromManaPool.Add((Mana) obj);
                    return true;
                }
                return false;
            }
            return false;
        }


        // http://mtgsalvation.gamepedia.com/Resolving_Spells_and_Abilities
        public void Resolve(IStackCard stackCard)
        {
            game.PostData($"Resolving spell {stackCard}");

            var card = stackCard as Card;

            var validEffects = true;

            // 608.2. If the object that’s resolving is an instant spell, a sorcery spell, or an ability, its resolution may involve several steps. The steps described in rules 608.2a and 608.2b are followed first. The steps described in rules 608.2c–j are then followed as appropriate, in no specific order. The step described in rule 608.2k is followed last.

            // 608.2a If a triggered ability has an intervening “if” clause, it checks whether the clause’s condition is true. If it isn’t, the ability is removed from the stack and does nothing. Otherwise, it continues to resolve. See rule 603.4.

            // 608.2b If the spell or ability specifies targets, it checks whether the targets are still legal. A target that’s no longer in the zone it was in when it was targeted is illegal. 
            if (card.isType(CardType.Sorcery) || card.isType(CardType.Instant) || card.isType(CardType.Ability))
            {
                validEffects = card.Abilities.Validate(game, card.Owner, card);
            }

            // 608.2c The Controller of the spell or ability follows its instructions in the order written.

            // 608.2d If an effect of a spell or ability offers any choices other than choices already made as part of casting the spell, activating the ability, or otherwise putting the spell or ability on the stack, the player announces these while applying the effect.

            // 608.2f If an effect gives a player the option to pay mana, he or she may activate mana abilities before taking that action. If an effect specifically instructs or allows a player to cast a spell during resolution, he or she does so by following the steps in rules 601.2a–i, except no player receives priority after it’s cast. That spell becomes the topmost object on the stack, and the currently resolving spell or ability continues to resolve, which may include casting other spells this way. No other spells can normally be cast and no other abilities can normally be activated during resolution.

            // 608.2g If an effect requires information from the game (such as the number of creatures on the battlefield), the answer is determined only once, when the effect is applied. If the effect requires information from a specific object, including the Source of the ability itself or a target that’s become illegal, the effect uses the current information of that object if it’s in the public zone it was expected to be in; if it’s no longer in that zone, or if the effect has moved it from a public zone to a hidden zone, the effect uses the object’s last known information. See rule 112.7a. If an ability states that an object does something, it’s the object as it exists—or as it most recently existed—that does it, not the ability.

            // 608.2h If an effect refers to certain characteristics, it checks only for the value of the specified characteristics, regardless of any related ones an object may also have.

            // 608.2i If an ability’s effect refers to a specific untargeted object that has been previously referred to by that ability’s cost or trigger condition, it still affects that object even if the object has changed characteristics.

            // 608.2j If an instant spell, sorcery spell, or ability that can legally resolve leaves the stack once it starts to resolve, it will continue to resolve fully.
            if (card.isType(CardType.Sorcery) || card.isType(CardType.Instant) || card.isType(CardType.Ability))
            {
                if (!validEffects)
                    game.PostData($"{card} fizzles because of no legal targets.");
                else
                {
                    var info = new BaseEventInfo {Game = game, sourceCard = card, sourcePlayer = card.Owner};
                    foreach (var ability in card.Abilities)
                        foreach (var effect in ability.effects)
                            effect.Invoke(info);
                }
            }

            // 608.3. If the object that’s resolving is a permanent spell, its resolution involves a single step (unless it’s an Aura). The spell card becomes a permanent and is put onto the battlefield under the control of the spell’s Controller.
            if (card.isType(CardType.Permanent))
            {
                // 608.3a If the object that’s resolving is an Aura spell, its resolution involves two steps. First, it checks whether the target specified by its enchant ability is still legal, as described in rule 608.2b. (See rule 702.5, “Enchant.”) If so, the spell card becomes a permanent and is put onto the battlefield under the control of the spell’s Controller attached to the object it was targeting.
                // 608.3b If a permanent spell resolves but its Controller can’t put it onto the battlefield, that player puts it into its owner’s graveyard.
                game.Methods.ChangeZone(card, Zone.Stack, Zone.Battlefield);
            }

            // 608.2k As the final part of an instant or sorcery spell’s resolution, the spell is put into its owner’s graveyard. As the final part of an ability’s resolution, the ability is removed from the stack and ceases to exist.
            else if (!card.isType(CardType.Ability))
            {
                if (card.isType(CardType.Sorcery) || card.isType(CardType.Instant))
                    game.Methods.ChangeZone((Card) stackCard, Zone.Stack, Zone.Graveyard);
            }
        }
    }
}