//ScrapskinDrake
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ScrapskinDrake : Creature 
    {
        public ScrapskinDrake() : base("Creature � Zombie Drake 2/3, 2U (3)")
        {
            Name = "Scrapskin Drake";
            Current.Text = @"Flying (This creature can't be blocked except by creatures with flying or reach.) Scrapskin Drake can block only creatures with flying.";
            throw new NotImplementedException();
        }
    }
}
