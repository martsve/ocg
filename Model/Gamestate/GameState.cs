using System;
using Delver.StateMachine;

namespace Delver.GameStates
{
    [Serializable]
    internal class PriorityState : GameState
    {
        public override GameState Handle(Context Context)
        {
            if (Context.CurrentStep.PriorityPlayer != null)
            {
                Context.CurrentStep.Interact();
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
        public override GameState Handle(Context Context)
        {
            if (Context.CurrentStep.order.Count > 0)
            {
                return new PriorityState();
            }
            if (Context.CurrentStep.stack.Count > 0)
            {
                Context.Logic.ResolveStack();
                return new PriorityState();
            }
            return null;
        }

        public override void Enter(Context Context)
        {
            Context.CurrentStep = Context.CurrentTurn.steps.Pop();
            Context.PostData("> " + Context.CurrentStep);
            Context.CurrentStep.Enter();
        }

        public override void Exit(Context Context)
        {
            Context.CurrentStep.Exit();
        }
    }

    [Serializable]
    internal class TurnState : GameState
    {
        public override GameState Handle(Context Context)
        {
            if (Context.CurrentTurn.steps.Count > 0)
                return new StepState();

            return null;
        }

        public override void Enter(Context Context)
        {
            BeginTurn(Context);
        }


        private void BeginTurn(Context Context)
        {
            if (Context.Turns.Count == 0)
                Context.Turns.Push(new Turn(Context));

            var turn = Context.Turns.Pop();
            Context.ActivePlayer = turn.Player;

            Context.CurrentTurn = turn;

            Context.TurnNumber++;

            if (Context.TurnNumber == 1)
            {
                Context.Methods.DrawHands();
                Context.Methods.CheckForMuligans();
            }

            Context.PostData($"Player {turn.Player} - Turn {Context.TurnNumber}");
        }
    }

    [Serializable]
    internal class BeginState : GameState
    {
        public override GameState Handle(Context Context)
        {
            return new TurnState();
        }

        public override void Enter(Context Context)
        {
            Context.Logic.InitializeGame();
        }
    }


    [Serializable]
    internal class InitState : GameState
    {
        public override GameState Handle(Context Context)
        {
            return new BeginState();
        }
    }

    [Serializable]
    internal abstract class GameState
    {
        public abstract GameState Handle(Context Context);

        public virtual void Enter(Context Context)
        {
        }

        public virtual void Exit(Context Context)
        {
        }

        public static GameState StateMachineCallback(StateMachineAction action, Context Context, GameState state)
        {
            if (action == StateMachineAction.Handle)
            {
                return state.Handle(Context);
            }
            if (action == StateMachineAction.Enter)
            {
                state.Enter(Context);
                return null;
            }
            if (action == StateMachineAction.Exit)
            {
                state.Exit(Context);
                return null;
            }
            throw new NotImplementedException();
        }
    }
}