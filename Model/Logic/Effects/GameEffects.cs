﻿using System;
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

        public override void Invoke(BaseEventInfo e)
        {
            source.Enchant(e, Targets.Single());
        }
    }

    [Serializable]
    internal class CallbackEffect : Effect
    {
        Action<BaseEventInfo> _callback;

        public CallbackEffect(Action<BaseEventInfo> callback, params ITarget[] targets) 
        {
            this._callback = callback;
            AddTarget(targets);
        }

        public override void Invoke(BaseEventInfo e)
        {
            _callback.Invoke(e);
        }
    }
}