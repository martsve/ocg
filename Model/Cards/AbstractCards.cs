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
            Base.CardAbilities.Add(ability);
            this.Source = source;
            Base.SetColor(source.Current.Color);

            ApplyBase();
        }

        public Card Source { get; set; }

        public override string ToString()
        {
            return string.Join(" , ", Current.CardAbilities.Select(x => x.ToString()));
        }
    }

    [Serializable]
    internal abstract class CreatureToken : Creature
    {
        public CreatureToken(int power, int thoughness) : base("", power, thoughness)
        {
            Base.Supertype.Add("Token");
            Base.AddType(CardType.Token);
        }
    }

    [Serializable]
    internal abstract class Land : Card
    {
        protected Land() : base(CardType.Land | CardType.Permanent)
        {
        }

        protected Land(CardType cardType) : this()
        {
            Base.AddType(cardType);
        }
    }

    [Serializable]
    internal abstract class Creature : Spell
    {
        protected Creature(string cost, int power, int thoughness) : base(CardType.Creature | CardType.Permanent)
        {
            Base.SetCastingCost(cost);
            Base.Power = power;
            Base.Thoughness = thoughness;
        }
    }

    [Serializable]
    internal abstract class Enchantment : Spell
    {
        protected Enchantment(string cost) : base(CardType.Enchantment)
        {
            Base.SetCastingCost(cost);
        }
    }

    [Serializable]
    internal abstract class Aura : Enchantment
    {
        protected Aura(string cost) : base(cost)
        {
            Base.Subtype.Add("Aura");
            throw new NotImplementedException();
        }
    }

    [Serializable]
    internal class Instant : Spell
    {
        public Instant(string castingCost) : base(CardType.Instant)
        {
            Base.SetCastingCost(castingCost);
        }
    }

    [Serializable]
    internal class Sorcery : Spell
    {
        public Sorcery(string castingCost) : base(CardType.Sorcery)
        {
            Base.SetCastingCost(castingCost);
        }
    }

}