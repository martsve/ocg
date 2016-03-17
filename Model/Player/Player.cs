using System;
using System.Collections.Generic;
using Delver.Interface;

namespace Delver
{
    [Serializable]
    internal class Player : GameObject
    {
        public Request request;

        public Player(Game game, string name, List<Card> library, Func<InputRequest, string> func = null)
        {
            Initialize(game);
            Name = name;

            foreach (var c in library)
            {
                c.SetOwner(this);
                Library.Add(c);
            }

            request = new Request(game, this, func);
        }

        public string Name { get; }

        public bool IsPlaying { get; set; } = true;

        public int Life { get; set; }
        public int Poision { get; set; }

        public int Mulligans { get; set; } = 0;

        public ManaCost ManaPool { get; set; } = new ManaCost();

        public ManaCost SelectedFromManaPool { get; set; } = new ManaCost();

        public List<Card> Hand { get; set; } = new List<Card>();
        public List<Card> Battlefield { get; set; } = new List<Card>();
        public List<Card> Library { get; set; } = new List<Card>();
        public List<Card> Exile { get; set; } = new List<Card>();
        public List<Card> Graveyard { get; set; } = new List<Card>();
        public List<Card> Command { get; set; } = new List<Card>();

        public int HandLimit
        {
            get { return 7; }
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public bool CanPlaySorcery(Game game)
        {
            return (game.CurrentStep.stack.Count == 0)
                && (game.CurrentStep.PriorityPlayer == this)
                && (game.CurrentStep.type == StepType.PostMain || game.CurrentStep.type == StepType.PreMain);
        }
    }
}