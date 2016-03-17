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
        public Context Context;
        public StepType type;

        public GameStep(Context Context, StepType type)
        {
            IsCombatStep = false;
            this.Context = Context;
            this.type = type;
        }

        public List<IStackCard> stack { get; set; } = new List<IStackCard>();

        public Stack<Player> order { get; set; } = new Stack<Player>();
        public Player PriorityPlayer { get; set; }

        public Stack<Player> priority { get; set; } = new Stack<Player>();

        public bool IsCombatStep { get; protected set; }

        public virtual void Enter()
        {
        }

        public virtual void Interact()
        {
            Context.Logic.Interact();
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