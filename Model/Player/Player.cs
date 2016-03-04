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
            Initializse(game);

            Name = name;
            Mulligans = 0;
            IsPlaying = true;

            Hand = new List<Card>();
            Battlefield = new List<Card>();
            Library = new List<Card>();
            Exile = new List<Card>();
            Graveyard = new List<Card>();
            Command = new List<Card>();

            ManaPool = new ManaCost();
            SelectedFromManaPool = new ManaCost();

            Marks = new Dictionary<string, int>();

            foreach (var c in library)
            {
                c.SetOwner(this);
                Library.Add(c);
            }

            request = new Request(game, this, func);
        }

        public string Name { get; }

        public bool IsPlaying { get; set; }

        public int Life { get; set; }
        public int Poision { get; set; }

        public int Mulligans { get; set; }

        public ManaCost ManaPool { get; set; }

        public ManaCost SelectedFromManaPool { get; set; }

        public List<Card> Hand { get; set; }
        public List<Card> Battlefield { get; set; }
        public List<Card> Library { get; set; }
        public List<Card> Exile { get; set; }
        public List<Card> Graveyard { get; set; }
        public List<Card> Command { get; set; }

        public int HandLimit
        {
            get { return 7; }
        }

        public Dictionary<string, int> Marks { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}