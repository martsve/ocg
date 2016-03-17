//PoreOverthePages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class PoreOverthePages : Sorcery 
    {
        public PoreOverthePages() : base("Sorcery, 3UU (5)")
        {
            Name = "Pore Over the Pages";
            Current.Text = @"Draw three cards, untap up to two lands, then discard a card.";
            throw new NotImplementedException();
        }
    }
}
