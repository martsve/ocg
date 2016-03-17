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
        public DiregrafGhoul() : base("B", 2, 2)
        {
            Name = "Diregraf Ghoul";
            Base.Subtype.Add("Zombie");
            Base.Text = @"Diregraf Ghoul enters the battlefield tapped.";
            throw new NotImplementedException();
        }
    }
}
