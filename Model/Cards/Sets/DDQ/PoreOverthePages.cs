//PoreOverthePages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class PoreOverthePages : Sorcery 
    {
        public PoreOverthePages() : base("3UU")
        {
            Name = "Pore Over the Pages";
            Base.Text = @"Draw three cards, untap up to two lands, then discard a card.";
            NotImplemented();
        }
    }
}
