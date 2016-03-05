using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{
    internal interface IStackCard
    {
        Player Controller { get; set; }
        void Resolve(Game game);
    }

    [Serializable]
    internal class Spell : Card, IStackCard
    {
        public Spell(CardType type) : base(type)
        {
        }

        public void Resolve(Game game)
        {
            game.Logic.Resolve(this);
        }
    }

    internal interface IImaginaryCard
    {
        Card Source { get; set; }
    }

    [Serializable]
    internal class AbilitySpell : Spell, IImaginaryCard
    {
        public AbilitySpell(Game game, Player player, Card source, Ability ability) : base(CardType.Ability)
        {
            SetOwner(player);
            Abilities.Add(ability);
            this.Source = source;
            SetColor(source.Color);
        }

        public Card Source { get; set; }

        public override string ToString()
        {
            return string.Join(" , ", Abilities.Select(x => x.ToString()));
        }
    }

    [Serializable]
    internal abstract class CreatureToken : Creature
    {
        public CreatureToken(Game game, Player owner, int power, int thoughness) : base("", power, thoughness)
        {
            game.Methods.AbsorbEvents(this);
            Initializse(game);
            SetOwner(owner);
            AddType(CardType.Token);
        }
    }

    [Serializable]
    internal abstract class Land : Card
    {
        protected Land() : base(CardType.Land | CardType.Permanent)
        {
            BasicType = new List<Identity>();
        }

        protected Land(CardType cardType) : this()
        {
            AddType(cardType);
        }

        public List<Identity> BasicType { get; set; }
    }

    [Serializable]
    internal abstract class Creature : Spell
    {
        protected Creature(int power, int thoughness) : base(CardType.Creature | CardType.Permanent)
        {
            BasePower = power;
            BaseThoughness = thoughness;
        }

        protected Creature(string cost, int power, int thoughness) : this(power, thoughness)
        {
            SetCastingCost(cost);
        }
    }
}