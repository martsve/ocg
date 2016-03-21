//BarterinBlood
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class BarterinBlood : Sorcery 
    {
        public BarterinBlood() : base("2BB")
        {
            Name = "Barter in Blood";
            Base.Text = @"Each player sacrifices two creatures.";
            NotImplemented();
        }
    }
}
