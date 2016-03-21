using System;
using Delver.Interface;
using Delver.StateMachine;
using Delver.View;

namespace Delver.GameStates
{
    [Serializable]
    internal class PriorityState : GameState
    {
        public override GameState Handle(Context context)
        {
            if (context.CurrentStep.PriorityPlayer != null)
            {
                context.CurrentStep.Interact();
                return new PriorityState();
            }
            return null;
        }

        public override void Enter(Context Context)
        {
            Context.Logic.SetPriorityPlayer();
        }
    }

    [Serializable]
    internal class StepState : GameState
    {
        public override GameState Handle(Context context)
        {
            if (context.CurrentStep.order.Count > 0)
            {
                return new PriorityState();
            }
            if (context.CurrentStep.stack.Count > 0)
            {
                context.Logic.ResolveStack();
                return new PriorityState();
            }
            return null;
        }

        public override void Enter(Context context)
        {
            context.CurrentStep = context.CurrentTurn.steps.Pop();
            MessageBuilder.CurrentStep(context).Send(context);
            context.CurrentStep.Enter();
        }

        public override void Exit(Context context)
        {
            context.CurrentStep.Exit();
        }
    }

    [Serializable]
    internal class TurnState : GameState
    {
        public override GameState Handle(Context context)
        {
            if (context.CurrentTurn.steps.Count > 0)
                return new StepState();

            return null;
        }

        public override void Enter(Context context)
        {
            BeginTurn(context);
        }


        private void BeginTurn(Context context)
        {
            if (context.Turns.Count == 0)
                context.Turns.Push(new Turn(context));

            var turn = context.Turns.Pop();
            context.ActivePlayer = turn.Player;

            context.CurrentTurn = turn;

            context.TurnNumber++;

            if (context.TurnNumber == 1)
            {
                context.Methods.DrawHands();
                context.Methods.CheckForMuligans();
            }

            MessageBuilder.BeginTurn(context).Send(context);
        }
    }

    [Serializable]
    internal class BeginState : GameState
    {
        public override GameState Handle(Context context)
        {
            return new TurnState();
        }

        public override void Enter(Context context)
        {
            context.Logic.InitializeGame();
        }
    }


    [Serializable]
    internal class InitState : GameState
    {
        public override GameState Handle(Context context)
        {
            return new BeginState();
        }
    }

    [Serializable]
    internal abstract class GameState
    {
        public abstract GameState Handle(Context context);

        public virtual void Enter(Context context)
        {
        }

        public virtual void Exit(Context context)
        {
        }

        public static GameState StateMachineCallback(StateMachineAction action, Context context, GameState state)
        {
            if (action == StateMachineAction.Handle)
            {
                return state.Handle(context);
            }
            else if (action == StateMachineAction.Enter)
            {
                state.Enter(context);
                return null;
            }
            else //if (action == StateMachineAction.Exit)
            {
                state.Exit(context);
                return null;
            }
        }
    }
}