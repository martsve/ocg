//StitchedDrake
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class StitchedDrake : Creature 
    {
        public StitchedDrake() : base("1UU", 3, 4)
        {
            Name = "Stitched Drake";
            Base.Subtype.Add("Zombie");
            Base.Subtype.Add("Drake");
            Base.Text = @"As an additional cost to cast Stitched Drake, exile a creature card from your graveyard. Flying";
            NotImplemented();
        }
    }
}
