//HarvesterofSouls
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class HarvesterofSouls : Creature 
    {
        public HarvesterofSouls() : base("4BB", 5, 5)
        {
            Name = "Harvester of Souls";
            Base.Subtype.Add("Demon");
            Base.AddKeyword(Keywords.Deathtouch);

            Base.When(
                 $"Whenever another nontoken creature dies, you may draw a card.",
                 EventCollection.CreatureDies(e => e.SourceCard != this && !e.SourceCard.IsSuperType("Token")),
                 e => e.Context.Methods.DrawCard(e.SourcePlayer, this)
             );

            // TODO implement MAY
        }
    }
}
