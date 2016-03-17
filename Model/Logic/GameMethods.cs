using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;
using System.Dynamic;
using System.Runtime.Remoting.Messaging;

namespace Delver
{
    [Serializable]
    internal class GameMethods
    {

        private readonly Context Context;
        private EventCollector EventCollector { get; set; }

        public GameMethods(Context Context)
        {
            this.Context = Context;
            this.EventCollector = new EventCollector(Context);
        }

        public void AddPlayer(string name, List<Card> library, Func<InputRequest, string> func = null)
        {
            dynamic msg = new ExpandoObject();
            msg.Action = "AddPlayer";
            msg.Name = name;
            msg.Decksize = library.Count();

            Context.PostData(((object)msg).ToJson());

            var player = new Player(Context, name, library, func);
            player.Initialize(Context);
            Context.Players.Add(player);

            foreach (var c in library)
                c.Initialize(Context, player);

            SetStartingLife(player);
            ShuffleLibrary(player);
        }

        public void Shuffle(Player player, Zone from)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    player.Battlefield.Shuffle(Context.Rand);
                    break;
                case Zone.Command:
                    player.Command.Shuffle(Context.Rand);
                    break;
                case Zone.Exile:
                    player.Exile.Shuffle(Context.Rand);
                    break;
                case Zone.Graveyard:
                    player.Graveyard.Shuffle(Context.Rand);
                    break;
                case Zone.Hand:
                    player.Hand.Shuffle(Context.Rand);
                    break;
                case Zone.Library:
                    player.Library.Shuffle(Context.Rand);
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
                    Context.CurrentStep.stack.Remove((IStackCard) card);
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
                    Context.CurrentStep.stack.Push((IStackCard) card);
                    break;

                case Zone.None:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void PlayerLoses(Player p, string message)
        {
            Context.PostData($"Player {p} lost the game: {message}");
            Context.SetRunning(false);
        }


        public void Tap(Card card)
        {
            Context.PostData("Tapping " + card);
            card.IsTapped = true;
        }

        public void Untap(Card card)
        {
            if (!card.Marks.ContainsKey(Marks.CANT_UNTAP))
            {
                Context.PostData("Untapping " + card);
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
            Context.PostData($"{player} starting life set to {Total}");
        }

        public void LoseLife(Player player, Card source, int N)
        {
            Context.PostData($"{player} loses {N} life from {source} ({source.Owner})");
            player.Life -= N;
        }

        public void GainLife(Player player, Card source, int N)
        {
            Context.PostData($"{player} gains {N} life from {source} ({source.Owner})");
            player.Life += N;
        }

        public void AddMana(Player player, Card source, Mana mana)
        {
            Context.PostData($"{player} adds {mana} to manapool from {source}");
            player.ManaPool.Add(mana);
            player.SelectedFromManaPool.Add(mana);
        }

        public void DrawCard(Player player, Card source = null, int N = 1)
        {
            Context.PostData($"{player} draws {N} cards");
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
            Context.Methods.PlayerLoses(player, message);
        }

        public void Discard(Player player, Card card, Card source = null)
        {
            Context.PostData($"{player} discards {card}");
            ChangeZone(card, Zone.Hand, Zone.Graveyard);
        }

        public void ShuffleLibrary(Player player, Card source = null)
        {
            Context.PostData($"{player} shuffles library");
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
            Context.PostData($"{source} ({source.Owner}) deals {N} damage to {target}");

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

            Context.PostData($"{source} ({source.Owner}) deals {N} damage to {target}");

            if (target is Player)
            {
                Context.Methods.TriggerEvents(new EventInfoCollection.DealsCombatDamageToPlayer(source, (Player) target, N, FirstStrike));
                DamagePlayer((Player) target, source, N);
            }
            else
            {
                Context.Methods.TriggerEvents(new EventInfoCollection.DealsCombatDamageToCreature(source, (Card) target, N, FirstStrike));
                DamageCreature((Card) target, source, N);
            }
        }

        public void DamagePlayer(Player player, Card source, int N)
        {
            LoseLife(player, source, N);
        }

        public void DamageCreature(Card card, Card source, int N)
        {
            if (card.isCardType(CardType.Creature) && N > 0)
            {
                card.Damage += N;

                if (source.Has(Keywords.Deathtouch))
                    card.DeathtouchDamage = true;
            }

        }


        public void ChangeZone(IEnumerable<Card> cards, Zone from, Zone to)
        {
            foreach (var card in cards.ToList())
                ChangeZone(card, from, to);
        }

        public bool TriggerReplacement(EventInfo info)
        {
            // TODO implement
            // throw new NotImplementedException();
            return false;
        }

        public void ChangeZone(Card card, Zone from, Zone to)
        {
            // TODO fake eventinfo
            if (Context.Methods.TriggerReplacement(new EventInfo() { triggerCard = card, FromZone = from, ToZone = to }))
                return;

            if (card.isCardType(CardType.Token) && to != Zone.None && from != Zone.None && from != Zone.Battlefield)
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

            card.SetZone(Context, from, to);

            Context.Methods.TriggerEvents(EventInfoCollection.LeaveZone(card, from, to));
            Context.Methods.TriggerEvents(EventInfoCollection.EnterZone(card, from, to));

            if (card.isCardType(CardType.Token) && from == Zone.Battlefield)
                Context.Logic.movedTokens.Add(card);

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
            Context.PostData($"{card} is destroyed by {source}");
            Die(card, from, false);
        }

        public void Die(Card card, Zone from, bool stateBased = true)
        {
            if (stateBased)
                Context.PostData($"{card} dies of state based effects");
            
            ChangeZone(card, from, Zone.Graveyard);

            Context.Methods.TriggerEvents(new EventInfoCollection.Dies(card));
        }


        public void Scry(Player player, Card source, int N)
        {
            var cards = player.Library.Take(N);
            player.Library.RemoveRange(0, N);

            var top = player.request.RequestMultiple(source, RequestType.Scry,
                "Select cards to put on top of library. Last card is put on bottom", cards, false);
            player.Library.InsertRange(0, top.Cast<Card>());
            Context.PostData($"Put {top.Count()} cards on top of library");

            if (top.Count < N)
            {
                var bot = player.request.RequestMultiple(source, RequestType.Scry,
                    "Arrange cards to put on bottom  of library. Last card is put on bottom",
                    cards.Where(x => !top.Contains(x)), true);
                player.Library.AddRange(bot.Cast<Card>());
                Context.PostData($"Put {bot.Count()} cards on bottom of library");
            }
        }

        public void DrawHands()
        {
            var N = GetStartHandsize();
            foreach (var p in Context.Players)
                DrawCard(p, null, N);
        }

        public void CheckForMuligans(int N = 0)
        {
            var mulls = 0;
            foreach (var p in Context.TurnOrder)
            {
                if (p.Mulligans >= N)
                    mulls += AskMulligan(p);
            }
            if (mulls > 0)
                CheckForMuligans(N + 1);
            else
            {
                foreach (var p in Context.TurnOrder)
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
                Context.PostData($"Drawing {N}");
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
                list = Context.Players;

            foreach (var p in Context.Players)
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
                    return Context.CurrentStep.stack.Where(c => c.Controller == player).Select(c => (Card) c);
                default:
                    throw new NotImplementedException();
            }
        }

        public void EmptyManaPools()
        {
            foreach (var p in Context.Players)
                EmptyManaPool(p);
        }

        public void EmptyManaPool(Player p)
        {
            p.ManaPool.Clear();
        }


        public void AddCounter(Card card, Counter counter)
        {
            card.Counters.Add(counter);
            counter.Add(Context, card);
        }

        public void RemoveCounters(Card card)
        {
            foreach (var counter in card.Counters.ToList())
            {
                counter.Remove();
                card.Counters.Remove(counter);
            }
        }

        public void AddEffectToStack(EventInfo e, Effect effect)
        {
            var ability = new Ability(effect);

            var abilitySpell = new AbilitySpell(Context, e.sourcePlayer, e.sourceCard, ability);

            AddAbilityToStack(abilitySpell);

            var result = PopulateResult.NoneSelected;
            while (result == PopulateResult.NoneSelected)
                result = ability.Populate(Context, e.triggerPlayer, e.triggerCard);

            if (result == PopulateResult.NoLegalTargets)
            {
                Context.PostData($"No legal targets for effect {effect}");

                // dirty way to remove added ability on stack
                Context.CurrentStep.stack.Pop();
            }

        }

        public void AddAbilityToStack(AbilitySpell card)
        {
            Context.CurrentStep.stack.Push(card);
        }

        public Card AddTokenAttacking(Player player, Card token)
        {
            token = AddToken(player, token);
            token.IsTapped = true;
            token.IsBlocked = false;
            token.IsAttacking = Context.Methods.SelectObjectToAttack(token);
            Context.Logic.attackers.Add(token);
            return token;
        }

        public Card AddToken(Player player, Card token)
        {
            token.Initialize(Context);
            token.SetOwner(player);
            Context.Methods.AbsorbEvents(token);
            Context.Methods.ChangeZone(token, Zone.None, Zone.Battlefield);
            return token;
        }

        public GameObject SelectObjectToAttack(Card attacker)
        {
            var ap = Context.Logic.attacker;
            var legalAttackedObjects = Context.Logic.defender.Battlefield.Where(x => x.isCardType(CardType.Planeswalker))
                .Select(x => (GameObject)x)
                .Union(new List<GameObject> { Context.Logic.defender })
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






        public void TriggerEvents(EventInfo e)
        {
            EventCollector.TriggerEvents(e);
        }

        public void CollectEvents()
        {
            EventCollector.CollectEvents();
        }

        public void AddDelayedTrigger(Card source, EventListener e)
        {
            EventCollector.AddDelayedTrigger(source, e);
        }

        public void AddEvents(Card card, Zone zone)
        {
            EventCollector.AddEvents(card, zone);
        }

        public void RemoveEvents(Card card, Zone zone)
        {
            EventCollector.RemoveEvents(card, zone);
        }

        public void RemoveEvents(Card card)
        {
            EventCollector.RemoveEvents(card);
        }

        public void RemoveEvents(EventListener e)
        {
            EventCollector.RemoveEvents(e);
        }


        public void AbsorbEvents(Card card)
        {
            EventCollector.AbsorbEvents(card);
        }

        public void ReleaseEvents()
        {
            EventCollector.ReleaseEvents();
        }
    }

    [Serializable]
    class EventCollector
    {
        private Context Context;
        public EventCollector(Context Context)
        {
            this.Context = Context;
        }

        private List<EventInfo> CollectedEvents { get; set; } = new List<EventInfo>();
        private List<EventListener> EventCollection { get; set; } = new List<EventListener>();
        private int EventPreventionCounter { get; set; }

        public void TriggerEvents(EventInfo e)
        {
            e.Context = Context;
            CollectedEvents.Add(e);
            if (EventPreventionCounter == 0)
                ReleaseEvents();
        }

        public void CollectEvents()
        {
            EventPreventionCounter++;
        }

        public void AddDelayedTrigger(Card source, EventListener e)
        {
            e.Source = source;
            e.IsDelayed = true;
            EventCollection.Add(e);
        }

        public void AddEvents(Card card, Zone zone)
        {
            foreach (var e in card.Current.Events.Where(x => x.EventInfo.ValidInZone == zone))
            {
                e.Source = card;
                EventCollection.Add(e);
            }
        }

        public void RemoveEvents(Card card, Zone zone)
        {
            foreach (var e in card.Current.Events.Where(x => x.EventInfo.ValidInZone == zone))
                DelayedEventRemoval.Add(e);
        }

        public void RemoveEvents(Card card)
        {
            foreach (var e in card.Current.Events)
                DelayedEventRemoval.Add(e);
        }

        public void RemoveEvents(EventListener e)
        {
            EventCollection.Remove(e);
        }


        public void AbsorbEvents(Card card)
        {
            foreach (var e in card.Current.Events.Where(x => x.EventInfo.ValidInZone == Zone.Global))
            {
                e.Source = card;
                EventCollection.Add(e);
            }
        }

        private List<EventListener> DelayedEventRemoval { get; set; } = new List<EventListener>();
        private void PurgeDelayedEventRemoval()
        {
            foreach (var e in DelayedEventRemoval)
                EventCollection.Remove(e);
            DelayedEventRemoval.Clear();
        }

        // http://mtgsalvation.gamepedia.com/Triggered_ability
        public void ReleaseEvents()
        {
            if (EventPreventionCounter > 0)
            {
                EventPreventionCounter--;
                if (EventPreventionCounter > 0)
                    return;
            }

            if (!CollectedEvents.Any())
                return;

            var matchingEventHandlers = new List<EventListener>();
            foreach (var e in CollectedEvents)
            {
                var items = EventCollection
                    .Where(x => x.Match(e))
                    .Select(x => x.AdoptTrigger(e)).ToList();

                var filtered = items.Where(x => x.Filter()).ToList();

                if (filtered.Any())
                    matchingEventHandlers.AddRange(filtered);
            }

            var groups = matchingEventHandlers.GroupBy(x => x.EventInfo.sourcePlayer).ToList();

            // 603.3. Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority. See rule 116, “Timing and Priority.” 
            // The ability becomes the topmost object on the stack. It has the text of the ability that created it, and no other characteristics. It remains on the stack until it’s countered, it resolves, a rule causes it to be removed from the stack, or an effect moves it elsewhere.
            // 603.3b If multiple abilities have triggered since the last time a player received priority, each player, in APNAP order, puts triggered abilities he or she controls on the stack in any order he or she chooses. (See rule 101.4.) Then the game once again checks for and resolves state-based actions until none are performed, then abilities that triggered during this process go on the stack. This process repeats until no new state-based actions are performed and no abilities trigger. Then the appropriate player gets priority.
            foreach (var p in Context.Logic.GetPriorityOrder())
            {
                var playersEvents = groups.FirstOrDefault(x => x.Key == p)?.Select(x => x);
                if (playersEvents != null)
                {
                    if (playersEvents.Count() > 1)
                        playersEvents =
                            p.request.RequestMultiple(null, RequestType.OrderTriggers,
                                $"{p}: Select order for abilities to go on the stack. Last one goes on top of the stack.",
                                playersEvents).Cast<EventListener>();

                    foreach (var handler in playersEvents)
                    {
                        var effect = handler.Effect;
                        Context.Methods.AddEffectToStack(handler.EventInfo, effect);
                    }
                }
            }

            PurgeDelayedEventRemoval();
            CollectedEvents.Clear();
        }

    }

}