//ButcherGhoul
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class ButcherGhoul : Creature 
    {
        public ButcherGhoul() : base("1B",1,1)
        {
            Name = "Butcher Ghoul";
            Base.Subtype.Add("Zombie");
            Base.AddKeyword(Keywords.Undying);
        }
    }
}
