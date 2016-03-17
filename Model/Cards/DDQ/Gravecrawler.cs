//Gravecrawler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class Gravecrawler : Creature 
    {
        public Gravecrawler() : base("Creature � Zombie 2/1, B (1)")
        {
            Name = "Gravecrawler";
            Current.Text = @"Gravecrawler can't block. You may cast Gravecrawler from your graveyard as long as you control a Zombie.";
            throw new NotImplementedException();
        }
    }
}
