//NephaliaSmuggler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class NephaliaSmuggler : Creature 
    {
        public NephaliaSmuggler() : base("Creature � Human Rogue 1/1, U (1)")
        {
            Name = "Nephalia Smuggler";
            Current.Text = @"{3}{U}, {T}: Exile another target creature you control, then return that card to the battlefield under your control. My drivers are trustworthy. I removed their tongues myself. Any other questions?";
            throw new NotImplementedException();
        }
    }
}
