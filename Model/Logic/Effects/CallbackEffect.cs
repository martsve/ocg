using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    internal class Effects
    {
        public static Effect Callback(Action<BaseEventInfo> callback, params ITarget[] targets)
        {
            var effect = new CallbackEffect(callback, targets);
            return effect;
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
