using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{

    [Serializable]
    internal class CustomEventHandler
    {
        public CustomEventHandler(BaseEventInfo info, Effect effect)
        {
            _originalEvent = this;
            filter = x => true;
            this.info = info;
            this.effect = effect;
        }

        public bool IsDelayed { get; set; }
        public Effect effect { get; set; }
        internal Func<BaseEventInfo, bool> filter { get; set; }
        public BaseEventInfo info { get; set; }

        private CustomEventHandler _originalEvent;

        internal Card source { get; set; }
        public string Text { get; set; }

        public bool Match(BaseEventInfo info)
        {
            return this.info.Match(info);
        }

        public bool Filter(BaseEventInfo info)
        {
            var match = filter(info);
            if (match && IsDelayed)
                info.Game.Methods.EventCollection.Remove(_originalEvent);
            return match;
        }

        public void Invoke(BaseEventInfo info)
        {
            effect.PerformEffect(info, source);
        }

        public CustomEventHandler Clone(BaseEventInfo e)
        {
            var clone = (CustomEventHandler) MemberwiseClone();
            clone.info = e;
            clone._originalEvent = this;
            return clone;
        }

        public override string ToString()
        {
            if (Text == null)
                return $"{source}_Event_{info.GetType()}";
            return Text;
        }
    }


    [Serializable]
    internal class EventCollection
    {
        public static CustomEventHandler BeginningOfEndStep(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.BeginningOfEndStep(zone), null);
            handler.filter = e => true;
            return handler;
        }

        public static CustomEventHandler EndOfCombatStep(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.EndOfCombatStep(zone), null);
            handler.filter = e => true;
            return handler;
        }

        public static CustomEventHandler ThisDies(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.Dies(zone), null);
            handler.filter = e => e.triggerCard == e.sourceCard;
            return handler;
        }

        public static CustomEventHandler CreatureEnterTheBattlefield(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.EnterTheBattlefield(zone), null);
            handler.filter = e => e.triggerCard.isCardType(CardType.Creature);
            return handler;
        }

        public static CustomEventHandler ThisLeavesTheBattlefield(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.EnterTheBattlefield(zone), null);
            handler.filter = e => e.triggerCard == e.sourceCard;
            return handler;
        }

        public static CustomEventHandler ThisEnterTheBattlefield(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.EnterTheBattlefield(zone), null);
            handler.filter = e => e.triggerCard == e.sourceCard;
            return handler;
        }

        public static CustomEventHandler ThisAttacks(Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new CustomEventHandler(new EventInfo.CreatureAttacks(zone), null);
            handler.filter = e => e.triggerCard == e.sourceCard;
            return handler;
        }

    }


    [Serializable]
    internal class BaseEventInfo
    {
        public BaseEventInfo()
        {
        }

        public BaseEventInfo(Zone type)
        {
            zone = type;
        }

        public Zone zone { get; set; }

        public Game Game { get; set; }

        public Card sourceCard { get; set; }
        public Player sourcePlayer { get; set; }
        public Card triggerCard { get; set; }
        public Player triggerPlayer { get; set; }
        public GameObjectReferance Following { get; set; }

        public GameObjectReferance Enchanted => Following.Card?.Current.EnchantedObject;

        public List<GameObject> Targets { get; set; }

        public bool Match(BaseEventInfo info)
        {
            var match = (zone == Zone.Global || zone == info.zone) && GetType().IsSameOrSubclass(info.GetType());
            return match;
        }

        public BaseEventInfo Clone(Card source)
        {
            var clone = (BaseEventInfo) MemberwiseClone();
            clone.sourceCard = source;
            clone.sourcePlayer = source.Controller;
            return clone;
        }

        public void AddDelayedTrigger(string text, CustomEventHandler handler, Action<BaseEventInfo> callback, params ITarget[] targets)
        {
            handler.Text = text;
            var effect = new CallbackEffect(callback);
            effect.AddTarget(targets);
            handler.effect = effect;
            Game.Methods.AddDelayedTrigger(sourceCard, handler);
        }

        public void AddToken(Card token, Player player = null)
        {
            Game.Methods.AddToken(player ?? triggerPlayer, token);
        }
    }

    [Serializable]
    internal class EventInfo
    {
        public static BaseEventInfo LeaveZone(Game game, Card card, Zone zone)
        {
            switch (zone)
            {
                case Zone.Battlefield:
                    return new LeaveTheBattlefield(game, card);
                case Zone.Command:
                    return new LeaveCommandzone(game, card);
                case Zone.Exile:
                    return new LeaveExile(game, card);
                case Zone.Graveyard:
                    return new LeaveGraveyard(game, card);
                case Zone.Hand:
                    return new LeaveHand(game, card);
                case Zone.Library:
                    return new LeaveLibrary(game, card);
                case Zone.Stack:
                    return new LeaveStack(game, card);

                case Zone.None:
                    return new BaseEventInfo();

                default:
                    throw new NotImplementedException();
            }
        }


        public static BaseEventInfo EnterZone(Game game, Card card, Zone zone)
        {
            switch (zone)
            {
                case Zone.Battlefield:
                    return new EnterTheBattlefield(game, card);
                case Zone.Command:
                    return new EnterCommandzone(game, card);
                case Zone.Exile:
                    return new EnterExile(game, card);
                case Zone.Graveyard:
                    return new EnterGraveyard(game, card);
                case Zone.Hand:
                    return new EnterHand(game, card);
                case Zone.Library:
                    return new EnterLibrary(game, card);
                case Zone.Stack:
                    return new EnterStack(game, card);

                case Zone.None:
                    return new BaseEventInfo();

                default:
                    throw new NotImplementedException();
            }
        }


        [Serializable]
        public class DealsCombatDamageToPlayer : BaseEventInfo
        {
            public int Damage;
            public bool FirstStrikeDamage;
            public Player target;

            public DealsCombatDamageToPlayer(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public DealsCombatDamageToPlayer(Game game, Card source, Player target, int N, bool FirstStrikeDamage)
                : this()
            {
                Game = game;
                triggerCard = source;

                this.target = target;
                Damage = N;
                this.FirstStrikeDamage = FirstStrikeDamage;
            }
        }

        [Serializable]
        public class DealsCombatDamageToCreature : BaseEventInfo
        {
            public int Damage;
            public bool FirstStrikeDamage;
            public Card target;

            public DealsCombatDamageToCreature(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public DealsCombatDamageToCreature(Game game, Card source, Card target, int N, bool FirstStrikeDamage)
                : this()
            {
                Game = game;
                triggerCard = source;

                this.target = target;
                Damage = N;
                this.FirstStrikeDamage = FirstStrikeDamage;
            }
        }

        [Serializable]
        public class Dies : BaseEventInfo
        {
            public Dies(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public Dies(Game game, Card card) : this()
            {
                Game = game;
                triggerPlayer = card.Controller;
                triggerCard = card;
            }
        }

        [Serializable]
        public class BeginningOfNextCleanupStep : BaseEventInfo
        {
            public BeginningOfNextCleanupStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfNextCleanupStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfPostMainStep : BaseEventInfo
        {
            public BeginningOfPostMainStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfPostMainStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class CreatureAttacks : BaseEventInfo
        {
            public CreatureAttacks(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CreatureAttacks(Game game, Player attacker, Player defender, Card card) : this()
            {
                Game = game;
                triggerPlayer = attacker;
                triggerCard = card;
            }
        }

        [Serializable]
        public class CreatuerBlocks : BaseEventInfo
        {
            private List<Card> Blocked;
            private Card Blocker;

            public CreatuerBlocks(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CreatuerBlocks(Game game, Player attacker, Player defender, Card Blocker, List<Card> Blocked)
                : this()
            {
                Game = game;
                triggerPlayer = defender;
                this.Blocker = Blocker;
                this.Blocked = Blocked;
            }
        }


        [Serializable]
        public class AttackersDeclared : BaseEventInfo
        {
            private List<Card> Cards;

            public AttackersDeclared(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public AttackersDeclared(Game game, Player attacker, Player defender, List<Card> cards) : this()
            {
                Game = game;
                triggerPlayer = attacker;
                Cards = cards;
            }
        }

        [Serializable]
        public class BlockersDeclared : BaseEventInfo
        {
            private List<Card> Cards;

            public BlockersDeclared(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BlockersDeclared(Game game, Player attacker, Player defender, List<Card> cards) : this()
            {
                Game = game;
                triggerPlayer = defender;
                Cards = cards;
            }
        }

        [Serializable]
        public class CombatDamageStep : BaseEventInfo
        {
            public CombatDamageStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CombatDamageStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class EndOfCombatStep : BaseEventInfo
        {
            public EndOfCombatStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EndOfCombatStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfEndStep : BaseEventInfo
        {
            public BeginningOfEndStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfEndStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfMainStep : BaseEventInfo
        {
            public BeginningOfMainStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfMainStep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfDrawstep : BaseEventInfo
        {
            public BeginningOfDrawstep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfDrawstep(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfCombatPhase : BaseEventInfo
        {
            public BeginningOfCombatPhase(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfCombatPhase(Game game, Player player) : this()
            {
                Game = game;
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfUpkeep : BaseEventInfo
        {
            public BeginningOfUpkeep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfUpkeep(Game game, Player activePlayer) : this()
            {
                Game = game;
                triggerPlayer = activePlayer;
            }
        }

        [Serializable]
        public class EnterTheBattlefield : BaseEventInfo
        {
            public EnterTheBattlefield(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterTheBattlefield(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class EnterCommandzone : BaseEventInfo
        {
            public EnterCommandzone(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterCommandzone(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class EnterExile : BaseEventInfo
        {
            public EnterExile(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterExile(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class EnterHand : BaseEventInfo
        {
            public EnterHand(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterHand(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class EnterLibrary : BaseEventInfo
        {
            public EnterLibrary(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterLibrary(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class EnterGraveyard : BaseEventInfo
        {
            public EnterGraveyard(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterGraveyard(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class EnterStack : BaseEventInfo
        {
            public EnterStack(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterStack(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class LeaveTheBattlefield : BaseEventInfo
        {
            public LeaveTheBattlefield(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveTheBattlefield(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class LeaveCommandzone : BaseEventInfo
        {
            public LeaveCommandzone(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveCommandzone(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class LeaveExile : BaseEventInfo
        {
            public LeaveExile(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveExile(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class LeaveHand : BaseEventInfo
        {
            public LeaveHand(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveHand(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }

        [Serializable]
        public class LeaveLibrary : BaseEventInfo
        {
            public LeaveLibrary(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveLibrary(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class LeaveGraveyard : BaseEventInfo
        {
            public LeaveGraveyard(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveGraveyard(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }


        [Serializable]
        public class LeaveStack : BaseEventInfo
        {
            public LeaveStack(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveStack(Game game, Card triggerCard) : this()
            {
                Game = game;
                this.triggerCard = triggerCard;
                triggerPlayer = triggerCard.Controller;
            }
        }
    }
}