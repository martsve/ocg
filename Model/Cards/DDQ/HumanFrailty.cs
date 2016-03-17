//HumanFrailty
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class HumanFrailty : Instant 
    {
        public HumanFrailty() : base("Instant, B (1)")
        {
            Name = "Human Frailty";
            Current.Text = @"Destroy target Human creature.";
            throw new NotImplementedException();
        }
    }
}
