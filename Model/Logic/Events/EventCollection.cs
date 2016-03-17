using System;

namespace Delver
{
    [Serializable]
    internal class EventCollection
    {
        public static EventHandler BeginningOfEndStep(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.BeginningOfEndStep(zone), null);
            handler.BaseFilter = e => true;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler EndOfCombatStep(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.EndOfCombatStep(zone), null);
            handler.BaseFilter = e => true;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler ThisDies(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.Dies(zone), null);
            handler.BaseFilter = e => e.triggerCard == e.sourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler CreatureEnterTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.EnterTheBattlefield(zone), null);
            handler.BaseFilter = e => e.triggerCard.isCardType(CardType.Creature);
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler ThisLeavesTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.LeaveTheBattlefield(zone), null);
            handler.BaseFilter = e => e.triggerCard == e.sourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler ThisEnterTheBattlefield(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.EnterTheBattlefield(zone), null);
            handler.BaseFilter = e => e.triggerCard == e.sourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

        public static EventHandler ThisAttacks(Func<EventInfo, bool> filter = null, Effect effect = null, Zone zone = Zone.Battlefield)
        {
            var handler = new EventHandler(new EventInfoCollection.CreatureAttacks(zone), null);
            handler.BaseFilter = e => e.triggerCard == e.sourceCard;
            handler.SpecialFilter = filter;
            return handler;
        }

    }
}