//AbattoirGhoul
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class AbattoirGhoul : Creature 
    {
        public AbattoirGhoul() : base("Creature � Zombie 3/2, 3B (4)")
        {
            Name = "Abattoir Ghoul";
            Current.Text = @"First strike Whenever a creature dealt damage by Abattoir Ghoul this turn dies, you gain life equal to that creature's toughness.";
            throw new NotImplementedException();
        }
    }
}
