using System;

namespace Delver.Effects
{
    [Serializable]
    internal class GainLifeEffect : Effect
    {
        private readonly int life;

        public GainLifeEffect(int life)
        {
            this.life = life;
            Text = $"You gain {life} life";
        }

        public override void Invoke(BaseEventInfo e)
        {
            e.Game.Methods.GainLife(e.sourcePlayer, e.sourceCard, life);
        }
    }


    [Serializable]
    internal class TriggerEffect : Effect
    {
        private readonly CustomEventHandler Event;

        public TriggerEffect(CustomEventHandler Event)
        {
            this.Event = Event;
        }

        public override void Invoke(BaseEventInfo info)
        {
            Event.Invoke(Event.info);
        }
    }
}