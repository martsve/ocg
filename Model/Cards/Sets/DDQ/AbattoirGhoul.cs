//AbattoirGhoul
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class AbattoirGhoul : Creature 
    {
        public AbattoirGhoul() : base("3B", 3, 2)
        {
            Name = "Abattoir Ghoul";
            Base.Subtype.Add("Zombie");
            Base.AddKeyword(Keywords.FirstStrike);
            
            NotImplemented();

            Base.When(
                "Whenever a creature dealt damage by Abattoir Ghoul this turn dies, you gain life equal to that creature's toughness.",
                EventCollection.CreatureDies(e => e.TriggerCard.DamagedBy.Contains(this.ZoneId)),
                e => e.Context.Methods.GainLife(e.SourcePlayer, e.SourceCard, e.TriggerCard.Current.Thoughness)
            );
        }
    }
}
