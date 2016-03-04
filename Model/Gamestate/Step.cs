using System;
using System.Collections.Generic;

namespace Delver
{
    internal enum StepType
    {
        Untap,
        Upkeep,
        Draw,
        PreMain,
        BeginCombat,
        SelectAttackers,
        SelectBlockers,
        CombatDamage,
        EndOfCombat,
        PostMain,
        End,
        Cleanup
    }

    [Serializable]
    internal abstract class GameStep
    {
        public Game game;
        public StepType type;

        public GameStep(Game game, StepType type)
        {
            IsCombatStep = false;
            stack = new List<IStackCard>();
            order = new Stack<Player>();
            priority = new Stack<Player>();
            this.game = game;
            this.type = type;
        }

        public List<IStackCard> stack { get; set; }

        public Stack<Player> order { get; set; }
        public Player PriorityPlayer { get; set; }

        public Stack<Player> priority { get; set; }

        public bool IsCombatStep { get; protected set; }

        public virtual void Enter()
        {
        }

        public virtual void Interact()
        {
            game.Logic.Interact();
        }

        public virtual void Exit()
        {
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }
}