//ScrapskinDrake
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class ScrapskinDrake : Creature 
    {
        public ScrapskinDrake() : base("2U", 2, 3)
        {
            Name = "Scrapskin Drake";
            Base.Subtype.Add("Zombie");
            Base.Subtype.Add("Drake");
            Base.AddKeyword(Keywords.Flying);
            Base.Text = @"Scrapskin Drake can block only creatures with flying.";
            NotImplemented();
        }
    }
}
