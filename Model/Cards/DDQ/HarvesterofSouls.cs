//HarvesterofSouls
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class HarvesterofSouls : Creature 
    {
        public HarvesterofSouls() : base("4BB", 5, 5)
        {
            Name = "Harvester of Souls";
            Base.Subtype.Add("Demon");
            Base.Text = @"Deathtouch Whenever another nontoken creature dies, you may draw a card.";
            throw new NotImplementedException();
        }
    }
}
