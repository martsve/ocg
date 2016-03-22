using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    [Serializable]
    internal class AbilitySpell : Spell
    {
        public AbilitySpell(Context Context, Player player, Card source, Ability ability, EventInfo baseEventInfo = null) : base(CardType.Ability)
        {
            this.BaseEventInfo = baseEventInfo;
            this.Initialize(Context);
            this.Source = source;
            SetOwner(player);
            Base.CardAbilities.Add(ability);
            Base.SetColor(source.Current.Color);
            ApplyBase();
        }

        public AbilitySpell(EventListener handler) : this(handler.EventInfo.Context, handler.EventInfo.SourcePlayer, handler.EventInfo.SourceCard, new Ability(handler.Effect), handler.EventInfo)
        {
        }

        public Card Source { get; set; }

        public override string ToString()
        {
            return string.Join(" , ", Current.CardAbilities.Select(x => x.ToString()));
        }
    }

}
