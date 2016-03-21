//NephaliaSmuggler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class NephaliaSmuggler : Creature 
    {
        public NephaliaSmuggler() : base("U", 1, 1)
        {
            Name = "Nephalia Smuggler";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Rogue");
            Base.Text = @"{3}{U}, {T}: Exile another target creature you control, then return that card to the battlefield under your control. My drivers are trustworthy. I removed their tongues myself. Any other questions?";
            NotImplemented();
        }
    }
}
