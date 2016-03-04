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
        private readonly Func<StateMachineAction, Game, T, T> Callback;
        private readonly Game game;
        private readonly Stack<T> state_ = new Stack<T>();

        public StateMachineManager(Game game, Func<StateMachineAction, Game, T, T> callback, T initialValue)
        {
            this.game = game;
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
            var state = Callback.Invoke(StateMachineAction.Handle, game, state_.Peek());
            if (state != null)
            {
                state_.Push(state);
                Callback.Invoke(StateMachineAction.Enter, game, state_.Peek());
            }
            else
            {
                Callback.Invoke(StateMachineAction.Exit, game, state_.Peek());
                state_.Pop();
            }
        }
    }
}