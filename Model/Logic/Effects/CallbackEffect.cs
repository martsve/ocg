using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver.Effects
{
    [Serializable]
    internal class CallbackEffect : Effect
    {
        Action<BaseEventInfo> _callback;
        public CallbackEffect(Action<BaseEventInfo> callback)
        {
            this._callback = callback;
        }
        public override void Invoke(BaseEventInfo e)
        {
            _callback.Invoke(e);
        }
    }

    [Serializable]
    internal class TargetedCallbackEffect : TargetedEffect
    {
        Action<BaseEventInfo, List<ITarget>> _callback;
        public TargetedCallbackEffect(Action<BaseEventInfo, List<ITarget>> callback, List<ITarget> targets)
        {
            this._callback = callback;
            targets.AddRange(targets);
        }

        public override void InvokeWhenValid(BaseEventInfo e)
        {
            _callback.Invoke(e, targets);
        }
    }

}
