using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    [Serializable]
    internal class AuraEffect : Effect
    {
        private Aura source { get; set; }

        public AuraEffect(Aura source, params ITarget[] targets)
        {
            this.source = source;
            AddTarget(targets);
        }

        public override void Invoke(EventInfo e)
        {
            source.Enchant(e, Targets.Single());
        }
    }

    [Serializable]
    internal class CallbackEffect : Effect
    {
        Action<EventInfo> _callback;

        public CallbackEffect(Action<EventInfo> callback, params ITarget[] targets) 
        {
            this._callback = callback;
            AddTarget(targets);
        }

        public override void Invoke(EventInfo e)
        {
            _callback.Invoke(e);
        }
    }
}
