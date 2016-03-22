using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver
{
    [Serializable]
    internal class Player : GameObject
    {
        public Request request;

        public Player(Context Context, string name, List<Card> library, Func<InputRequest, string> func = null)
        {
            Initialize(Context);
            Name = name;

            foreach (var c in library)
            {
                c.SetOwner(this);
                Library.Add(c);
            }

            request = new Request(Context, this, func);
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
        public List<Card> Temporary { get; set; } = new List<Card>();

        public int HandLimit
        {
            get { return 7; }
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public bool CanPlaySorcery(Context context)
        {
            return (context.CurrentStep.stack.Count == 0)
                && (context.CurrentStep.PriorityPlayer == this)
                && (context.CurrentStep.type == StepType.PostMain || context.CurrentStep.type == StepType.PreMain);
        }

        public bool HasDelirium()
        {
            int types = 0;

            var allCardTypes = new List<CardType> {
                CardType.Artifact,
                CardType.Creature,
                CardType.Enchantment,
                CardType.Instant,
                CardType.Land,
                CardType.Planeswalker,
                CardType.Sorcery,
                CardType.Tribal,
                CardType.Conspiracy,
                CardType.Phenomenon,
                CardType.Plane,
                CardType.Scheme,
                CardType.Vanguard,
            };

            foreach (var type in allCardTypes)
                if (Graveyard.Any(x => x.isCardType(type)))
                    types++;

            return types >= 4;
        }
    }
}