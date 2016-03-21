//DiregrafCaptain
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class DiregrafCaptain : Creature 
    {
        public DiregrafCaptain() : base("1UB", 2, 2)
        {
            Name = "Diregraf Captain";
            Base.Subtype.Add("Zombie");
            Base.Subtype.Add("Soldier");
            Base.Text = @"Deathtouch Other Zombie creatures you control get +1/+1. Whenever another Zombie you control dies, target opponent loses 1 life.";
            //throw new NotImplementedException();
        }
    }
}
