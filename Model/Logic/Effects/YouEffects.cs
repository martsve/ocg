using System;

namespace Delver
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

    // We make this redunant class to allow the game to be serialized. If we used an Action<> callback that would not be possible...
    [Serializable]
    internal class TriggerEffect : Effect
    {
        private readonly CustomEventHandler _event;

        public TriggerEffect(CustomEventHandler Event)
        {
            this._event = Event;
        }

        public override void Invoke(BaseEventInfo e)
        {
            // ignore e, we already have base event info from triggereffect creation
            _event.Invoke(_event.info);
        }
    }

}