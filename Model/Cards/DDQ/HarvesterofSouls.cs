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
        public HarvesterofSouls() : base("Creature � Demon 5/5, 4BB (6)")
        {
            Name = "Harvester of Souls";
            Current.Text = @"Deathtouch Whenever another nontoken creature dies, you may draw a card.";
            throw new NotImplementedException();
        }
    }
}
