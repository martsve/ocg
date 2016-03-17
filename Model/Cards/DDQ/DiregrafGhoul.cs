//DiregrafGhoul
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class DiregrafGhoul : Creature 
    {
        public DiregrafGhoul() : base("Creature � Zombie 2/2, B (1)")
        {
            Name = "Diregraf Ghoul";
            Current.Text = @"Diregraf Ghoul enters the battlefield tapped.";
            throw new NotImplementedException();
        }
    }
}
