using System;

namespace Delver
{
    enum EventType
    {
        BeginningOfEndStep,
        EndOfCombatStep,
        ThisDies,
        CreatureEnterTheBattlefield,
        ThisLeavesTheBattlefield,
        ThisEnterTheBattlefield,
        ThisAttacks,
    }


    /*[Serializable]
    class EventFactory
    {
        public static EventListener Create(EventType eventType, Func<EventInfo, bool> filter = null, Effect effect = null, Zone validZone = Zone.Battlefield)
        {
            var handler = new EventListener(eventType, validZone, null)
            {
                BaseFilter = e => true,
                SpecialFilter = filter
            };
            return handler;
            
        }
    }*/

    [Serializable]
    internal class EventCollection
    {
        public static EventListener BeginningOfEndStep(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.BeginningOfEndStep(zone), null);
            handler.BaseFilter = e => true;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener EndOfCombatStep(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.EndOfCombatStep(zone), null);
            handler.BaseFilter = e => true;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener ThisDies(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.Dies(zone), null);
            handler.BaseFilter = e => e.TriggerCard == e.SourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener CreatureDies(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.Dies(zone), null);
            handler.SpecialFilter = filter;
            return handler;
        }


        public static EventListener CreatureEnterTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.EnterTheBattlefield(zone), null);
            handler.BaseFilter = e => e.TriggerCard.isCardType(CardType.Creature);
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener ThisLeavesTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.LeaveTheBattlefield(zone), null);
            handler.BaseFilter = e => e.TriggerCard == e.SourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener ThisEnterTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.EnterTheBattlefield(zone), null);
            handler.BaseFilter = e => e.TriggerCard == e.SourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventListener ThisAttacks(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventListener(new EventInfoCollection.CreatureAttacks(zone), null);
            handler.BaseFilter = e => e.TriggerCard == e.SourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }
    }
}