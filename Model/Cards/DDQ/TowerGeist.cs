//TowerGeist
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class TowerGeist : Creature 
    {
        public TowerGeist() : base("Creature � Spirit 2/2, 3U (4)")
        {
            Name = "Tower Geist";
            Current.Text = @"Flying When Tower Geist enters the battlefield, look at the top two cards of your library. Put one of them into your hand and the other into your graveyard.";
            throw new NotImplementedException();
        }
    }
}
