//FalkenrathNoble
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class FalkenrathNoble : Creature 
    {
        public FalkenrathNoble() : base("Creature � Vampire 2/2, 3B (4)")
        {
            Name = "Falkenrath Noble";
            Current.Text = @"Flying Whenever Falkenrath Noble or another creature dies, target player loses 1 life and you gain 1 life.";
            throw new NotImplementedException();
        }
    }
}
