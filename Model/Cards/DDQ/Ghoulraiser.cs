//Ghoulraiser
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class Ghoulraiser : Creature 
    {
        public Ghoulraiser() : base("Creature � Zombie 2/2, 1BB (3)")
        {
            Name = "Ghoulraiser";
            Current.Text = @"When Ghoulraiser enters the battlefield, return a Zombie card at random from your graveyard to your hand.";
            throw new NotImplementedException();
        }
    }
}
