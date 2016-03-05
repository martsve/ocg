using System;
using System.Collections.Generic;

namespace Delver
{
    [Serializable]
    internal class CustomEventHandler
    {
        public CustomEventHandler(BaseEventInfo info, Action<BaseEventInfo> callback)
        {
            _originalEvent = this;
            filter = x => true;
            this.info = info;
            this.callback = callback;
        }

        public bool IsDelayed { get; set; }
        protected Action<BaseEventInfo> callback { get; }
        internal Func<BaseEventInfo, bool> filter { get; set; }
        public BaseEventInfo info { get; set; }

        private CustomEventHandler _originalEvent;

        internal Card source { get; set; }
        public string Text { get; set; }

        public bool Match(BaseEventInfo info)
        {
            return this.info.Match(info) && filter(info);
        }

        public virtual void Invoke(BaseEventInfo info)
        {
            callback(info.Clone(source));
            if (IsDelayed)
                info.Game.Methods.EventCollection.Remove(_originalEvent);
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
    internal class Events
    {
        [Serializable]
        public class EndOfCombatStep : CustomEventHandler
        {
            public EndOfCombatStep(Action<BaseEventInfo> callback)
                : base(new EventInfo.EndOfCombatStep(), callback)
            {
                filter = e => true;
            }
        }

        [Serializable]
        public class ThisEnterTheBattlefield : CustomEventHandler
        {
            public ThisEnterTheBattlefield(Action<BaseEventInfo> callback)
                : base(new EventInfo.EnterTheBattlefield(), callback)
            {
                filter = e => source.Zone == Zone.Battlefield && e.triggerCard == source;
            }
        }

        [Serializable]
        public class ThisAttacks : CustomEventHandler
        {
            public ThisAttacks(Action<BaseEventInfo> callback)
                : base(new EventInfo.CreatureAttacks(), callback)
            {
                filter = e => e.triggerCard == source;
            }
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

        public bool Match(BaseEventInfo info)
        {
            return (zone == Zone.Global || zone == info.zone) && GetType().IsSameOrSubclass(info.GetType());
        }

        public BaseEventInfo Clone(Card source)
        {
            var clone = (BaseEventInfo) MemberwiseClone();
            clone.sourceCard = source;
            clone.sourcePlayer = source.Controller;
            return clone;
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

            public DealsCombatDamageToPlayer() : base(Zone.Battlefield)
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

            public DealsCombatDamageToCreature() : base(Zone.Battlefield)
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
        public class BeginningOfNextCleanupStep : BaseEventInfo
        {
            public BeginningOfNextCleanupStep() : base(Zone.Battlefield)
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
            public BeginningOfPostMainStep() : base(Zone.Battlefield)
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
            public CreatureAttacks() : base(Zone.Battlefield)
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

            public CreatuerBlocks() : base(Zone.Battlefield)
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

            public AttackersDeclared() : base(Zone.Battlefield)
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

            public BlockersDeclared() : base(Zone.Battlefield)
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
            public CombatDamageStep() : base(Zone.Battlefield)
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
            public EndOfCombatStep() : base(Zone.Battlefield)
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
            public BeginningOfEndStep() : base(Zone.Battlefield)
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
            public BeginningOfMainStep() : base(Zone.Battlefield)
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
            public BeginningOfDrawstep() : base(Zone.Battlefield)
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
            public BeginningOfCombatPhase() : base(Zone.Battlefield)
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
            public BeginningOfUpkeep() : base(Zone.Battlefield)
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
            public EnterTheBattlefield() : base(Zone.Battlefield)
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
            public EnterCommandzone() : base(Zone.Battlefield)
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
            public EnterExile() : base(Zone.Battlefield)
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
            public EnterHand() : base(Zone.Battlefield)
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
            public EnterLibrary() : base(Zone.Battlefield)
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
            public EnterGraveyard() : base(Zone.Battlefield)
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
            public EnterStack() : base(Zone.Battlefield)
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
            public LeaveTheBattlefield() : base(Zone.Battlefield)
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
            public LeaveCommandzone() : base(Zone.Battlefield)
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
            public LeaveExile() : base(Zone.Battlefield)
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
            public LeaveHand() : base(Zone.Battlefield)
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
            public LeaveLibrary() : base(Zone.Battlefield)
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
            public LeaveGraveyard() : base(Zone.Battlefield)
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
            public LeaveStack() : base(Zone.Battlefield)
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