using System;
using System.Collections.Generic;

namespace Delver
{
    [Serializable]
    internal class EventInfoCollection
    {
        public static EventInfo LeaveZone(Card card, Zone from, Zone to)
        {
            switch (from)
            {
                case Zone.Battlefield:
                    return new LeaveTheBattlefield(card, to);
                case Zone.Command:
                    return new LeaveCommandzone(card, to);
                case Zone.Exile:
                    return new LeaveExile(card, to);
                case Zone.Graveyard:
                    return new LeaveGraveyard(card, to);
                case Zone.Hand:
                    return new LeaveHand(card, to);
                case Zone.Library:
                    return new LeaveLibrary(card, to);
                case Zone.Stack:
                    return new LeaveStack(card, to);

                case Zone.None:
                    return new EventInfo();

                default:
                    throw new NotImplementedException();
            }
        }


        public static EventInfo EnterZone(Card card, Zone from, Zone to)
        {
            switch (to)
            {
                case Zone.Battlefield:
                    return new EnterTheBattlefield(card, from);
                case Zone.Command:
                    return new EnterCommandzone(card, from);
                case Zone.Exile:
                    return new EnterExile(card, from);
                case Zone.Graveyard:
                    return new EnterGraveyard(card, from);
                case Zone.Hand:
                    return new EnterHand(card, from);
                case Zone.Library:
                    return new EnterLibrary(card, from);
                case Zone.Stack:
                    return new EnterStack(card, from);

                case Zone.None:
                    return new EventInfo();

                default:
                    throw new NotImplementedException();
            }
        }


        [Serializable]
        public class DealsCombatDamageToPlayer : EventInfo
        {
            public int Damage;
            public bool FirstStrikeDamage;
            public Player target;

            public DealsCombatDamageToPlayer(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public DealsCombatDamageToPlayer(Card source, Player target, int N, bool FirstStrikeDamage)
                : this()
            {
                triggerCard = source;

                this.target = target;
                Damage = N;
                this.FirstStrikeDamage = FirstStrikeDamage;
            }
        }

        [Serializable]
        public class DealsCombatDamageToCreature : EventInfo
        {
            public int Damage;
            public bool FirstStrikeDamage;
            public Card target;

            public DealsCombatDamageToCreature(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public DealsCombatDamageToCreature(Card source, Card target, int N, bool FirstStrikeDamage)
                : this()
            {
                triggerCard = source;

                this.target = target;
                Damage = N;
                this.FirstStrikeDamage = FirstStrikeDamage;
            }
        }

        [Serializable]
        public class Dies : EventInfo
        {
            public Dies(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public Dies(Card card) : this()
            {
                triggerPlayer = card.Controller;
                triggerCard = card;
            }
        }

        [Serializable]
        public class BeginningOfNextCleanupStep : EventInfo
        {
            public BeginningOfNextCleanupStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfNextCleanupStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfPostMainStep : EventInfo
        {
            public BeginningOfPostMainStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfPostMainStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class CreatureAttacks : EventInfo
        {
            public CreatureAttacks(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CreatureAttacks(Player attacker, Player defender, Card card) : this()
            {
                triggerPlayer = attacker;
                triggerCard = card;
            }
        }

        [Serializable]
        public class CreatuerBlocks : EventInfo
        {
            private List<Card> Blocked;
            private Card Blocker;

            public CreatuerBlocks(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CreatuerBlocks(Player attacker, Player defender, Card Blocker, List<Card> Blocked)
                : this()
            {
                triggerPlayer = defender;
                this.Blocker = Blocker;
                this.Blocked = Blocked;
            }
        }


        [Serializable]
        public class AttackersDeclared : EventInfo
        {
            private List<Card> Cards;

            public AttackersDeclared(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public AttackersDeclared(Player attacker, Player defender, List<Card> cards) : this()
            {
                triggerPlayer = attacker;
                Cards = cards;
            }
        }

        [Serializable]
        public class BlockersDeclared : EventInfo
        {
            private List<Card> Cards;

            public BlockersDeclared(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BlockersDeclared(Player attacker, Player defender, List<Card> cards) : this()
            {
                triggerPlayer = defender;
                Cards = cards;
            }
        }

        [Serializable]
        public class CombatDamageStep : EventInfo
        {
            public CombatDamageStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public CombatDamageStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class EndOfCombatStep : EventInfo
        {
            public EndOfCombatStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EndOfCombatStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfEndStep : EventInfo
        {
            public BeginningOfEndStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfEndStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfMainStep : EventInfo
        {
            public BeginningOfMainStep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfMainStep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfDrawstep : EventInfo
        {
            public BeginningOfDrawstep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfDrawstep(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfCombatPhase : EventInfo
        {
            public BeginningOfCombatPhase(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfCombatPhase(Player player) : this()
            {
                triggerPlayer = player;
            }
        }

        [Serializable]
        public class BeginningOfUpkeep : EventInfo
        {
            public BeginningOfUpkeep(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public BeginningOfUpkeep(Player activePlayer) : this()
            {
                triggerPlayer = activePlayer;
            }
        }

        [Serializable]
        public class EnterTheBattlefield : EventInfo
        {
            public EnterTheBattlefield(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterTheBattlefield(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Battlefield;;
            }
        }

        [Serializable]
        public class EnterCommandzone : EventInfo
        {
            public EnterCommandzone(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterCommandzone(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Command;
            }
        }


        [Serializable]
        public class EnterExile : EventInfo
        {
            public EnterExile(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterExile(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Exile;
            }
        }

        [Serializable]
        public class EnterHand : EventInfo
        {
            public EnterHand(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterHand(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Hand;
            }
        }

        [Serializable]
        public class EnterLibrary : EventInfo
        {
            public EnterLibrary(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterLibrary(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Library;
            }
        }


        [Serializable]
        public class EnterGraveyard : EventInfo
        {
            public EnterGraveyard(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterGraveyard(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Graveyard;
            }
        }


        [Serializable]
        public class EnterStack : EventInfo
        {
            public EnterStack(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public EnterStack(Card triggerCard, Zone from) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = from;
                this.ToZone = Zone.Stack;
            }
        }


        [Serializable]
        public class LeaveTheBattlefield : EventInfo
        {
            public LeaveTheBattlefield(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveTheBattlefield(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Battlefield;
                this.ToZone = to;
            }
        }

        [Serializable]
        public class LeaveCommandzone : EventInfo
        {
            public LeaveCommandzone(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveCommandzone(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Command;
                this.ToZone = to;
            }
        }


        [Serializable]
        public class LeaveExile : EventInfo
        {
            public LeaveExile(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveExile(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Exile;
                this.ToZone = to;
            }
        }

        [Serializable]
        public class LeaveHand : EventInfo
        {
            public LeaveHand(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveHand(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Hand;
                this.ToZone = to;
            }
        }

        [Serializable]
        public class LeaveLibrary : EventInfo
        {
            public LeaveLibrary(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveLibrary(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Library;
                this.ToZone = to;
            }
        }


        [Serializable]
        public class LeaveGraveyard : EventInfo
        {
            public LeaveGraveyard(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveGraveyard(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Graveyard;
                this.ToZone = to;
            }
        }


        [Serializable]
        public class LeaveStack : EventInfo
        {
            public LeaveStack(Zone zone = Zone.Battlefield) : base(zone)
            {
            }

            public LeaveStack(Card triggerCard, Zone to) : this()
            {
                this.triggerCard = triggerCard;
                this.triggerPlayer = triggerCard.Controller;
                this.FromZone = Zone.Stack;
                this.ToZone = to;
            }
        }
    }
}