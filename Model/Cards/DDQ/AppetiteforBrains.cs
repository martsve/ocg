//AppetiteforBrains
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class AppetiteforBrains : Sorcery 
    {
        public AppetiteforBrains() : base("Sorcery, B (1)")
        {
            Name = "Appetite for Brains";
            Current.Text = @"Target opponent reveals his or her hand. You choose a card from it with converted mana cost 4 or greater and exile that card.";
            throw new NotImplementedException();
        }
    }
}
