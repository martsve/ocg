using System;
using System.Linq;
using System.Security.AccessControl;

namespace Delver
{

    [Serializable]
    internal class EventListener
    {
        public EventListener(EventInfo eventInfo, Effect effect)
        {
            this.OriginalEvent = this;
            this.BaseFilter = x => true;
            this.EventInfo = eventInfo;
            this.Effect = effect;

        }

        public bool IsDelayed { get; set; }
        public Effect Effect { get; set; }
        internal Func<EventInfo, bool> BaseFilter { get; set; }
        internal Func<EventInfo, bool> SpecialFilter { get; set; }

        public EventInfo EventInfo { get; set; }

        private EventListener OriginalEvent { get; set; }

        internal Card Source { get; set; }
        public string Text { get; set; }

        public bool Match(EventInfo e)
        {
            return this.EventInfo.Match(e);
        }

        public bool Filter()
        {
            var match = BaseFilter(EventInfo) && (SpecialFilter == null || SpecialFilter(EventInfo));
            if (match && IsDelayed)
            {
                EventInfo.Context.Methods.RemoveEvents(OriginalEvent);
            }
            return match;
        }

        public void HandleEvent(EventInfo info)
        {
            Effect.PerformEffect(info, Source);
        }

        public EventListener AdoptTrigger(EventInfo eventInfo)
        {
            var newHandler = (EventListener)MemberwiseClone();
            var info = this.EventInfo.Clone(this.Source);
            info.TriggerCard = eventInfo.TriggerCard;
            info.TriggerPlayer = eventInfo.TriggerPlayer;
            info.Context = eventInfo.Context;
            newHandler.EventInfo = info;
            return newHandler;
        }

        public override string ToString()
        {
            if (Text == null)
                return $"{Source}_Event_{EventInfo.GetType()}";
            return Text;
        }
    }
}