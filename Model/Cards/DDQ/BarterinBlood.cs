//BarterinBlood
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class BarterinBlood : Sorcery 
    {
        public BarterinBlood() : base("Sorcery, 2BB (4)")
        {
            Name = "Barter in Blood";
            Current.Text = @"Each player sacrifices two creatures.";
            throw new NotImplementedException();
        }
    }
}
