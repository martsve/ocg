using System;
using System.Collections.Generic;

namespace Delver
{
    [Serializable]
    internal class EventInfo
    {
        public EventInfo()
        {
        }

        public EventInfo(Zone validZone)
        {
            ValidInZone = validZone;
        }

        public Zone ValidInZone { get; private set; }
        public Zone FromZone { get; set; }
        public Zone ToZone { get; set; }

        public Context Context { get; set; }

        public Card sourceCard { get; set; }
        public Player sourcePlayer { get; set; }
        public Card triggerCard { get; set; }
        public Player triggerPlayer { get; set; }
        public GameObjectReferance Following { get; set; }

        public GameObjectReferance Enchanted => Following.Card?.Current.EnchantedObject;

        public List<GameObject> Targets { get; set; }

        public bool Match(EventInfo info)
        {
            var match = GetType().IsSameOrSubclass(info.GetType());
            return match;
        }

        public EventInfo Clone(Card source)
        {
            var newInfo = (EventInfo)MemberwiseClone();
            newInfo.sourceCard = source;
            newInfo.sourcePlayer = source.Controller;
            return newInfo;
        }

        public void AddDelayedTrigger(string text, EventListener listener, Action<EventInfo> callback, params ITarget[] targets)
        {
            listener.Text = text;
            var effect = new CallbackEffect(callback);
            effect.AddTarget(targets);
            listener.Effect = effect;
            Context.Methods.AddDelayedTrigger(sourceCard, listener);
        }

        public void AddToken(Card token, Player player = null)
        {
            Context.Methods.AddToken(player ?? sourcePlayer, token);
        }
    }
}