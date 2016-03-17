using System;
using System.Collections.Generic;

namespace Delver.StateMachine
{
    [Serializable]
    internal enum StateMachineAction
    {
        Enter,
        Exit,
        Handle
    }

    [Serializable]
    internal class StateMachineManager<T>
    {
        private readonly Func<StateMachineAction, Context, T, T> Callback;
        private readonly Context Context;
        private readonly Stack<T> state_ = new Stack<T>();

        public StateMachineManager(Context Context, Func<StateMachineAction, Context, T, T> callback, T initialValue)
        {
            this.Context = Context;
            Callback = callback;
            state_ = new Stack<T>();
            state_.Push(initialValue);
        }

        public void Run(Func<bool> Running)
        {
            while (Running.Invoke())
                ProgressState();
        }

        private void ProgressState()
        {
            var state = Callback.Invoke(StateMachineAction.Handle, Context, state_.Peek());
            if (state != null)
            {
                state_.Push(state);
                Callback.Invoke(StateMachineAction.Enter, Context, state_.Peek());
            }
            else
            {
                Callback.Invoke(StateMachineAction.Exit, Context, state_.Peek());
                state_.Pop();
            }
        }
    }
}