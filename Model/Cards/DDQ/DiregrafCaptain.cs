//DiregrafCaptain
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class DiregrafCaptain : Creature 
    {
        public DiregrafCaptain() : base("Creature � Zombie Soldier 2/2, 1UB (3)")
        {
            Name = "Diregraf Captain";
            Current.Text = @"Deathtouch Other Zombie creatures you control get +1/+1. Whenever another Zombie you control dies, target opponent loses 1 life.";
            throw new NotImplementedException();
        }
    }
}
