//StitchedDrake
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class StitchedDrake : Creature 
    {
        public StitchedDrake() : base("Creature � Zombie Drake 3/4, 1UU (3)")
        {
            Name = "Stitched Drake";
            Current.Text = @"As an additional cost to cast Stitched Drake, exile a creature card from your graveyard. Flying";
            throw new NotImplementedException();
        }
    }
}
