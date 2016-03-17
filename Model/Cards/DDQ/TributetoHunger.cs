//TributetoHunger
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class TributetoHunger : Instant 
    {
        public TributetoHunger() : base("2B")
        {
            Name = "Tribute to Hunger";
            Base.Text = @"Target opponent sacrifices a creature. You gain life equal to that creature's toughness.";
            throw new NotImplementedException();
        }
    }
}
