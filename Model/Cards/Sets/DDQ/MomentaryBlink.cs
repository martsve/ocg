//MomentaryBlink
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MomentaryBlink : Instant 
    {
        public MomentaryBlink() : base("1W")
        {
            Name = "Momentary Blink";
            Base.Text = @"Exile target creature you control, then return it to the battlefield under its owner's control. Flashback {3}{U} (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
