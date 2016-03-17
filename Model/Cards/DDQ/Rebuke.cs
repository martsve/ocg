//Rebuke
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class Rebuke : Instant 
    {
        public Rebuke() : base("Instant, 2W (3)")
        {
            Name = "Rebuke";
            Current.Text = @"Destroy target attacking creature.";
            throw new NotImplementedException();
        }
    }
}
