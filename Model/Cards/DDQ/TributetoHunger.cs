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
        public TributetoHunger() : base("Instant, 2B (3)")
        {
            Name = "Tribute to Hunger";
            Current.Text = @"Target opponent sacrifices a creature. You gain life equal to that creature's toughness.";
            throw new NotImplementedException();
        }
    }
}
