//FalkenrathNoble
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class FalkenrathNoble : Creature 
    {
        public FalkenrathNoble() : base("3B", 2, 2)
        {
            Name = "Falkenrath Noble";
            Base.Subtype.Add("Vampire");
            Base.Text = @"Flying Whenever Falkenrath Noble or another creature dies, target player loses 1 life and you gain 1 life.";
            //throw new NotImplementedException();
        }
    }
}
