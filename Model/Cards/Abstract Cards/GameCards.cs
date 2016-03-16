using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
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

}
