//CompellingDeterrence
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class CompellingDeterrence : Instant 
    {
        public CompellingDeterrence() : base("Instant, 1U (2)")
        {
            Name = "Compelling Deterrence";
            Current.Text = @"Return target nonland permanent to its owner's hand. Then that player discards a card if you control a Zombie.";
            throw new NotImplementedException();
        }
    }
}
