//Gravecrawler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class Gravecrawler : Creature 
    {
        public Gravecrawler() : base("B", 2, 1)
        {
            Name = "Gravecrawler";
            Base.Subtype.Add("Zombie");
            Base.Text = @"Gravecrawler can't block. You may cast Gravecrawler from your graveyard as long as you control a Zombie.";
            NotImplemented();
        }
    }
}
