//CompellingDeterrence
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class CompellingDeterrence : Instant 
    {
        public CompellingDeterrence() : base("1U")
        {
            Name = "Compelling Deterrence";
            Base.Text = @"Return target nonland permanent to its owner's hand. Then that player discards a card if you control a Zombie.";
            NotImplemented();
        }
    }
}
