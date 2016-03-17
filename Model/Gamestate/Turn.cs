using System;
using System.Collections.Generic;
using System.Linq;
using Delver.GameSteps;

namespace Delver
{
    [Serializable]
    internal class Turn
    {
        private readonly Context Context;

        public int LandsPlayed = 0;
        public Player Player;
        public List<GameStep> steps;

        public Turn(Context Context)
        {
            this.Context = Context;
            Player = Context.Logic.GetNextPlayer();
            steps = GetTurnSteps();
            Context.CurrentStep = steps.First();
        }

        public Turn(Context Context, Player Player)
            : this(Context)
        {
            this.Player = Player;
        }

        public int CombatDamagePhase { get; set; }


        public void AdditionalCombatPhase(GameStep afterStep = null)
        {
            var combat = GetCombatPhase();

            if (afterStep == null)
                steps.InsertRange(0, combat);

            else
            {
                for (var i = 0; i < steps.Count(); i++)
                {
                    if (steps[i].GetType().IsSameOrSubclass(afterStep.GetType()))
                    {
                        steps.InsertRange(i + 1, combat);
                        break;
                    }
                }
            }
        }

        private List<GameStep> GetCombatPhase()
        {
            var steps = new List<GameStep>();
            steps.Add(new BeginCombatStep(Context));
            steps.Add(new SelectAttackers(Context));
            steps.Add(new SelectBlockers(Context));
            steps.Add(new CombatDamage(Context));
            steps.Add(new EndCombatStep(Context));
            return steps;
        }

        private List<GameStep> GetTurnSteps()
        {
            var steps = new List<GameStep>();
            steps.Add(new UntapStep(Context));
            steps.Add(new UpkeepStep(Context));
            steps.Add(new DrawStep(Context));
            steps.Add(new PreMainPhase(Context));

            foreach (var s in GetCombatPhase())
                steps.Add(s);

            steps.Add(new PostMainPhase(Context));
            steps.Add(new EndStep(Context));
            steps.Add(new CleanupStep(Context));
            return steps;
        }
    }
}