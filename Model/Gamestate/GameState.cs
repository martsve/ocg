using System;
using Delver.StateMachine;

namespace Delver.GameStates
{
    [Serializable]
    internal class PriorityState : GameState
    {
        public override GameState Handle(Game game)
        {
            if (game.CurrentStep.PriorityPlayer != null)
            {
                game.CurrentStep.Interact();
                return new PriorityState();
            }
            return null;
        }

        public override void Enter(Game game)
        {
            game.Logic.SetPriorityPlayer();
        }
    }

    [Serializable]
    internal class StepState : GameState
    {
        public override GameState Handle(Game game)
        {
            if (game.CurrentStep.order.Count > 0)
            {
                return new PriorityState();
            }
            if (game.CurrentStep.stack.Count > 0)
            {
                game.Logic.ResolveStack();
                return new PriorityState();
            }
            return null;
        }

        public override void Enter(Game game)
        {
            game.CurrentStep = game.CurrentTurn.steps.Pop();
            game.PostData("> " + game.CurrentStep);
            game.CurrentStep.Enter();
        }

        public override void Exit(Game game)
        {
            game.CurrentStep.Exit();
        }
    }

    [Serializable]
    internal class TurnState : GameState
    {
        public override GameState Handle(Game game)
        {
            if (game.CurrentTurn.steps.Count > 0)
                return new StepState();

            return null;
        }

        public override void Enter(Game game)
        {
            BeginTurn(game);
        }


        private void BeginTurn(Game game)
        {
            if (game.Turns.Count == 0)
                game.Turns.Push(new Turn(game));

            var turn = game.Turns.Pop();
            game.ActivePlayer = turn.Player;

            game.CurrentTurn = turn;

            game.TurnNumber++;

            if (game.TurnNumber == 1)
            {
                game.Methods.DrawHands();
                game.Methods.CheckForMuligans();
            }

            game.PostData($"Player {turn.Player} - Turn {game.TurnNumber}");
        }
    }

    [Serializable]
    internal class BeginState : GameState
    {
        public override GameState Handle(Game game)
        {
            return new TurnState();
        }

        public override void Enter(Game game)
        {
            game.Logic.InitializeGame();
        }
    }


    [Serializable]
    internal class InitState : GameState
    {
        public override GameState Handle(Game game)
        {
            return new BeginState();
        }
    }

    [Serializable]
    internal abstract class GameState
    {
        public abstract GameState Handle(Game game);

        public virtual void Enter(Game game)
        {
        }

        public virtual void Exit(Game game)
        {
        }

        public static GameState StateMachineCallback(StateMachineAction action, Game game, GameState state)
        {
            if (action == StateMachineAction.Handle)
            {
                return state.Handle(game);
            }
            if (action == StateMachineAction.Enter)
            {
                state.Enter(game);
                return null;
            }
            if (action == StateMachineAction.Exit)
            {
                state.Exit(game);
                return null;
            }
            throw new NotImplementedException();
        }
    }
}