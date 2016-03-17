//ButcherGhoul
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ButcherGhoul : Creature 
    {
        public ButcherGhoul() : base("1B",1,1)
        {
            Name = "Butcher Ghoul";
            Base.Subtype.Add("Zombie");
            Base.Text = @"Undying (When this creature dies, if it had no +1/+1 counters on it, return it to the battlefield under its owner's control with a +1/+1 counter on it.)";
            throw new NotImplementedException();
        }
    }
}
