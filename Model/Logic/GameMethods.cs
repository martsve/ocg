using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;
using System.Dynamic;

namespace Delver
{
    [Serializable]
    internal class GameMethods
    {
        private List<EventTriggerWrapper> _collectedEvents = new List<EventTriggerWrapper>();

        public List<CustomEventHandler> EventCollection { get; set; }

        public int EventPreventionCounter { get; set; }
        private readonly Game game;

        public GameMethods(Game game)
        {
            this.game = game;
            EventCollection = new List<CustomEventHandler>();
        }

        public void AddPlayer(string name, List<Card> library, Func<InputRequest, string> func = null)
        {
            dynamic msg = new ExpandoObject();
            msg.Action = "AddPlayer";
            msg.Name = name;
            msg.Decksize = library.Count();

            game.PostData(((object)msg).ToJson());

            var p = new Player(game, name, library, func);
            p.Initializse(game);
            game.Players.Add(p);

            foreach (var c in library)
            {
                c.Initializse(game);
                c.Owner = p;
                AbsorbEvents(c);
            }

            SetStartingLife(p);
            ShuffleLibrary(p);
        }

        public void Shuffle(Player player, Zone from)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    player.Battlefield.Shuffle(game.Rand);
                    break;
                case Zone.Command:
                    player.Command.Shuffle(game.Rand);
                    break;
                case Zone.Exile:
                    player.Exile.Shuffle(game.Rand);
                    break;
                case Zone.Graveyard:
                    player.Graveyard.Shuffle(game.Rand);
                    break;
                case Zone.Hand:
                    player.Hand.Shuffle(game.Rand);
                    break;
                case Zone.Library:
                    player.Library.Shuffle(game.Rand);
                    break;

                case Zone.Stack:
                default:
                    throw new NotImplementedException();
            }
        }


        public void Remove(Card card, Zone from)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    card.Controller.Battlefield.Remove(card);
                    break;
                case Zone.Command:
                    card.Controller.Command.Remove(card);
                    break;
                case Zone.Exile:
                    card.Controller.Exile.Remove(card);
                    break;
                case Zone.Graveyard:
                    card.Controller.Graveyard.Remove(card);
                    break;
                case Zone.Hand:
                    card.Controller.Hand.Remove(card);
                    break;
                case Zone.Library:
                    card.Controller.Library.Remove(card);
                    break;

                case Zone.Stack:
                    game.CurrentStep.stack.Remove((IStackCard) card);
                    break;

                case Zone.None:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Add(Player player, Card card, Zone from)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    player.Battlefield.Add(card);
                    break;
                case Zone.Command:
                    player.Command.Add(card);
                    break;
                case Zone.Exile:
                    player.Exile.Add(card);
                    break;
                case Zone.Graveyard:
                    player.Graveyard.Add(card);
                    break;
                case Zone.Hand:
                    player.Hand.Add(card);
                    break;
                case Zone.Library:
                    player.Library.Add(card);
                    break;
                case Zone.Stack:
                    game.CurrentStep.stack.Push((IStackCard) card);
                    break;

                case Zone.None:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void PlayerLoses(Player p, string message)
        {
            game.PostData($"Player {p} lost the game: {message}");
            game.SetRunning(false);
        }


        public void Tap(Card card)
        {
            game.PostData("Tapping " + card);
            card.IsTapped = true;
        }

        public void Untap(Card card)
        {
            if (!card.Marks.ContainsKey(Marks.CANT_UNTAP))
            {
                game.PostData("Untapping " + card);
                card.IsTapped = false;
            }
        }

        public void Exile(GameObjectReferance cardref)
        {
            var card = cardref.Card;
            if (card != null)
            {
                ChangeZone(card, card.Zone, Zone.Exile); 
            }
        }

        public void SetStartingLife(Player player)
        {
            var Total = 20;
            player.Life = Total;
            game.PostData($"{player} starting life set to {Total}");
        }

        public void LoseLife(Player player, Card source, int N)
        {
            game.PostData($"{player} loses {N} life from {source} ({source.Owner})");
            player.Life -= N;
        }

        public void GainLife(Player player, Card source, int N)
        {
            game.PostData($"{player} gains {N} life from {source} ({source.Owner})");
            player.Life += N;
        }

        public void AddMana(Player player, Card source, Mana mana)
        {
            game.PostData($"{player} adds {mana} to manapool from {source}");
            player.ManaPool.Add(mana);
            player.SelectedFromManaPool.Add(mana);
        }

        public void DrawCard(Player player, Card source = null, int N = 1)
        {
            game.PostData($"{player} draws {N} cards");
            for (var i = 0; i < N; i++)
            {
                if (player.Library.Count > 0)
                {
                    var card = player.Library.First();
                    ChangeZone(card, Zone.Library, Zone.Hand);
                }
                else
                {
                    LoseTheGame(player, "No more cards left in library to draw");
                }
            }
        }


        public void LoseTheGame(Player player, string message)
        {
            player.IsPlaying = false;
            game.Methods.PlayerLoses(player, message);
        }

        public void Discard(Player player, Card card, Card source = null)
        {
            game.PostData($"{player} discards {card}");
            ChangeZone(card, Zone.Hand, Zone.Graveyard);
        }

        public void ShuffleLibrary(Player player, Card source = null)
        {
            game.PostData($"{player} shuffles library");
            Shuffle(player, Zone.Library);
        }

        private int GetStartHandsize()
        {
            return 7;
        }

        public void AddMana(Player p, Card source, Identity color)
        {
            AddMana(p, source, new Mana(color));
        }

        public void DealDamage(Card source, GameObject target, int N)
        {
            game.PostData($"{source} ({source.Owner}) deals {N} damage to {target}");

            if (target is Player)
            {
                // Ask for redirect to planeswalker!
                DamagePlayer((Player) target, source, N);
            }
            else
                DamageCreature((Card) target, source, N);
        }


        public void DealCombatDamage(Card source, GameObject target, int N, bool FirstStrike = false)
        {
            if (N == 0)
                return;

            game.PostData($"{source} ({source.Owner}) deals {N} damage to {target}");

            if (target is Player)
            {
                game.Methods.TriggerEvents(new EventInfo.DealsCombatDamageToPlayer(game, source, (Player) target, N,
                    FirstStrike));
                DamagePlayer((Player) target, source, N);
            }
            else
            {
                game.Methods.TriggerEvents(new EventInfo.DealsCombatDamageToCreature(game, source, (Card) target, N,
                    FirstStrike));
                DamageCreature((Card) target, source, N);
            }
        }

        public void DamagePlayer(Player player, Card source, int N)
        {
            LoseLife(player, source, N);
        }

        public void DamageCreature(Card card, Card source, int N)
        {
            if (card.isType(CardType.Creature))
                card.Damage += N;
        }


        public void ChangeZone(IEnumerable<Card> cards, Zone from, Zone to)
        {
            foreach (var card in cards.ToList())
                ChangeZone(card, from, to);
        }

        public void ChangeZone(Card card, Zone from, Zone to)
        {
            if (card.isType(CardType.Token) && to != Zone.None && from != Zone.None && from != Zone.Battlefield)
                return;

            AddEvents(card, to);

            if (card.Controller == card.Owner)
            {
                Remove(card, from);
                Add(card.Controller, card, to);
            }
            else
            {
                if (from == Zone.Battlefield)
                {
                    // leaves battlefield - Controller loses controll
                    ChangeController(card, card.Owner, from, to);
                }
                else if (to == Zone.Battlefield)
                {
                    // enters under controllers controll
                    Remove(card, from);
                    Add(card.Controller, card, to);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            card.SetZone(game, from, to);

            game.Methods.TriggerEvents(EventInfo.LeaveZone(game, card, from));
            game.Methods.TriggerEvents(EventInfo.EnterZone(game, card, to));

            if (card.isType(CardType.Token) && from == Zone.Battlefield)
                game.Logic.movedTokens.Add(card);

            RemoveEvents(card, from);
        }

        public void ChangeController(Card card, Player newController, Zone from, Zone to)
        {
            Remove(card, from);
            Add(newController, card, to);
            throw new NotImplementedException();
        }

        public void Destroy(Card source, Card card, Zone from = Zone.Battlefield)
        {
            game.PostData($"{card} is destroyed by {source}");
            Die(card, from, false);
        }

        public void Die(Card card, Zone from, bool stateBased = true)
        {
            if (stateBased)
                game.PostData($"{card} dies of state based effects");
            
            ChangeZone(card, from, Zone.Graveyard);

            game.Methods.TriggerEvents(new EventInfo.Dies(game, card));
        }


        public void Scry(Player player, Card source, int N)
        {
            var cards = player.Library.Take(N);
            player.Library.RemoveRange(0, N);

            var top = player.request.RequestMultiple(source, RequestType.Scry,
                "Select cards to put on top of library. Last card is put on bottom", cards, false);
            player.Library.InsertRange(0, top.Cast<Card>());
            game.PostData($"Put {top.Count()} cards on top of library");

            if (top.Count < N)
            {
                var bot = player.request.RequestMultiple(source, RequestType.Scry,
                    "Arrange cards to put on bottom  of library. Last card is put on bottom",
                    cards.Where(x => !top.Contains(x)), true);
                player.Library.AddRange(bot.Cast<Card>());
                game.PostData($"Put {bot.Count()} cards on bottom of library");
            }
        }

        public void DrawHands()
        {
            var N = GetStartHandsize();
            foreach (var p in game.Players)
                DrawCard(p, null, N);
        }

        public void CheckForMuligans(int N = 0)
        {
            var mulls = 0;
            foreach (var p in game.TurnOrder)
            {
                if (p.Mulligans >= N)
                    mulls += AskMulligan(p);
            }
            if (mulls > 0)
                CheckForMuligans(N + 1);
            else
            {
                foreach (var p in game.TurnOrder)
                {
                    if (p.Mulligans > 0)
                        Scry(p, null, 1);
                }
            }
        }

        public int AskMulligan(Player p)
        {
            var action = p.request.RequestYesNo(RequestType.Mulligan, string.Format("Mulligan {0}? 1. Yes / 2. No", p));
            if (action.Type == InteractionType.Accept)
            {
                MulliganHand(p);
                return 1;
            }
            return 0;
        }

        public void MulliganHand(Player p)
        {
            p.Mulligans++;

            ChangeZone(p.Hand, Zone.Hand, Zone.Library);

            var N = GetStartHandsize() - p.Mulligans;
            if (N >= 1)
            {
                game.PostData($"Drawing {N}");
                DrawCard(p, null, N);
            }
        }


        public List<GameObject> GetAllTargets(TargetType type, Player player = null, Zone from = Zone.Battlefield)
        {
            var objs = new List<GameObject>();

            var list = new List<Player>();
            if (player != null)
                list.Add(player);
            else
                list = game.Players;

            foreach (var p in game.Players)
            {
                if (type.isType(TargetType.Card))
                {
                    foreach (var c in GetCards(p, from))
                        objs.Add(c);
                }

                if (type.isType(TargetType.Player))
                {
                    objs.Add(p);
                }

                if (type.isType(TargetType.Mana))
                {
                    foreach (var m in p.ManaPool)
                        objs.Add(m);
                }
            }

            return objs;
        }

        private IEnumerable<Card> GetCards(Player player, Zone from)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    return player.Battlefield;
                case Zone.Command:
                    return player.Command;
                case Zone.Exile:
                    return player.Exile;
                case Zone.Graveyard:
                    return player.Graveyard;
                case Zone.Hand:
                    return player.Hand;
                case Zone.Library:
                    return player.Library;
                case Zone.Stack:
                    return game.CurrentStep.stack.Where(c => c.Controller == player).Select(c => (Card) c);
                default:
                    throw new NotImplementedException();
            }
        }

        public void EmptyManaPools()
        {
            foreach (var p in game.Players)
                EmptyManaPool(p);
        }

        public void EmptyManaPool(Player p)
        {
            p.ManaPool.Clear();
        }


        public void AddCounter(Card card, Counter counter)
        {
            card.Counters.Add(counter);
            counter.Add(game, card);
        }

        public void RemoveCounters(Card card)
        {
            foreach (var counter in card.Counters.ToList())
            {
                counter.Remove();
                card.Counters.Remove(counter);
            }
        }

        public void AddEffectToStack(BaseEventInfo e, Effect effect)
        {
            var ability = new Ability(effect);

            var result = PopulateResult.NoneSelected;
            while (result == PopulateResult.NoneSelected)
                result = ability.Populate(game, e.triggerPlayer, e.triggerCard);

            if (result == PopulateResult.NoLegalTargets)
            {
                game.PostData($"No legal targets for effect {effect}");
                return;
            }

            var abilitySpell = new AbilitySpell(game, e.sourcePlayer, e.sourceCard, ability);

            e.Game.Methods.AddAbilityToStack(abilitySpell);
        }

        public void AddAbilityToStack(AbilitySpell card)
        {
            game.CurrentStep.stack.Push(card);
        }

        public void TriggerEvents(BaseEventInfo e)
        {
            TriggerEvents(new List<BaseEventInfo> {e});
        }

        public void TriggerEvents(IEnumerable<BaseEventInfo> eList)
        {
            foreach (var e in eList)
            {
                // find all matches
                var matches = EventCollection
                    .Where(x => x.Match(e))
                    .Select(x => new EventTriggerWrapper { handler = x, trigger = e });

                // set the source of the trigger
                foreach (var wrapper in matches)
                {
                    wrapper.trigger.sourceCard = wrapper.handler.source;
                    wrapper.trigger.sourcePlayer = wrapper.handler.source.Controller;
                }

                _collectedEvents.AddRange(matches);
            }

            if (EventPreventionCounter == 0)
                ReleaseEvents();
        }

        public void CollectEvents()
        {
            EventPreventionCounter++;
        }

        public void ReleaseEvents()
        {
            if (EventPreventionCounter > 0)
            {
                EventPreventionCounter--;
                if (EventPreventionCounter > 0)
                    return;
            }

            if (!_collectedEvents.Any())
                return;

            var matches = _collectedEvents.Where(x => x.handler.Filter(x.trigger)).ToList();
            var groups = matches.GroupBy(x => x.handler.source.Controller).ToList();

            foreach (var p in game.Logic.GetPriorityOrder())
            {
                var triggers = groups.FirstOrDefault(x => x.Key == p)?.Select(x => x);
                if (triggers != null)
                {
                    if (triggers.Count() > 1)
                        triggers =
                            p.request.RequestMultiple(null, RequestType.OrderTriggers,
                                $"{p}: Select order for abilities to go on the stack. Last one goes on top of the stack.",
                                triggers).Cast<EventTriggerWrapper>();

                    foreach (var hit in triggers)
                    {
                        var newEventHandler = hit.handler.Clone(hit.trigger);
                        var newEventInfo = hit.trigger.Clone(newEventHandler.source);
                        var effect = new TriggerEffect(newEventHandler) { Text = newEventHandler.Text };
                        hit.trigger.Game.Methods.AddEffectToStack(newEventInfo, effect);
                    }
                }
            }


            _collectedEvents = new List<EventTriggerWrapper>();
        }


        public Card AddTokenAttacking(Player player, Card token)
        {
            token = AddToken(player, token);
            token.IsTapped = true;
            token.IsBlocked = false;
            token.IsAttacking = game.Methods.SelectObjectToAttack(token);
            game.Logic.attackers.Add(token);
            return token;
        }

        public Card AddToken(Player player, Card token)
        {
            token.Initializse(game);
            token.SetOwner(player);
            game.Methods.AbsorbEvents(token);
            game.Methods.ChangeZone(token, Zone.None, Zone.Battlefield);
            return token;
        }

        public GameObject SelectObjectToAttack(Card attacker)
        {
            var ap = game.Logic.attacker;
            var legalAttackedObjects = game.Logic.defender.Battlefield.Where(x => x.isType(CardType.Planeswalker))
                .Select(x => (GameObject)x)
                .Union(new List<GameObject> { game.Logic.defender })
                .ToList();
            GameObject attacked_object = null;
            while (attacked_object == null)
            {
                if (legalAttackedObjects.Count() > 1)
                {
                    attacked_object = ap.request.RequestFromObjects(RequestType.Attacking,
                        $"Select object for {attacker} to attack", legalAttackedObjects);
                }
                else
                    attacked_object = legalAttackedObjects.First();
            }
            return attacked_object;
        }

        public void AddDelayedTrigger(Card source, CustomEventHandler e)
        {
            e.source = source;
            e.IsDelayed = true;
            EventCollection.Add(e);
        }

        public void AddEvents(Card card, Zone zone)
        {
            foreach (var e in card.Current.Events.Where(x => x.info.zone == zone))
            {
                e.source = card;
                EventCollection.Add(e);
            }
        }

        public void RemoveEvents(Card card, Zone zone)
        {
            foreach (var e in card.Current.Events.Where(x => x.info.zone == zone))
                EventCollection.Remove(e);
        }

        public void RemoveEvents(Card card)
        {
            foreach (var e in card.Current.Events)
                EventCollection.Remove(e);
        }

        public void AbsorbEvents(Card card)
        {
            foreach (var e in card.Current.Events.Where(x => x.info.zone == Zone.Global))
            {
                e.source = card;
                EventCollection.Add(e);
            }
        }


        [Serializable]
        private class EventTriggerWrapper
        {
            public CustomEventHandler handler;
            public BaseEventInfo trigger;
        }

    }
}