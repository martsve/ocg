using System;
using System.Collections.Generic;
using System.Linq;
using Delver.GameSteps;

namespace Delver
{
    [Serializable]
    internal class Turn
    {
        private readonly Game game;

        public int LandsPlayed = 0;
        public Player Player;
        public List<GameStep> steps;

        public Turn(Game game)
        {
            this.game = game;
            Player = game.Logic.GetNextPlayer();
            steps = GetTurnSteps();
            game.CurrentStep = steps.First();
        }

        public Turn(Game game, Player Player)
            : this(game)
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
            steps.Add(new BeginCombatStep(game));
            steps.Add(new SelectAttackers(game));
            steps.Add(new SelectBlockers(game));
            steps.Add(new CombatDamage(game));
            steps.Add(new EndCombatStep(game));
            return steps;
        }

        private List<GameStep> GetTurnSteps()
        {
            var steps = new List<GameStep>();
            steps.Add(new UntapStep(game));
            steps.Add(new UpkeepStep(game));
            steps.Add(new DrawStep(game));
            steps.Add(new PreMainPhase(game));

            foreach (var s in GetCombatPhase())
                steps.Add(s);

            steps.Add(new PostMainPhase(game));
            steps.Add(new EndStep(game));
            steps.Add(new CleanupStep(game));
            return steps;
        }
    }
}