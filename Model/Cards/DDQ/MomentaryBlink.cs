//MomentaryBlink
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class MomentaryBlink : Instant 
    {
        public MomentaryBlink() : base("Instant, 1W (2)")
        {
            Name = "Momentary Blink";
            Current.Text = @"Exile target creature you control, then return it to the battlefield under its owner's control. Flashback {3}{U} (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
