//HumanFrailty
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class HumanFrailty : Instant 
    {
        public HumanFrailty() : base("B")
        {
            Name = "Human Frailty";
            Base.Text = @"Destroy target Human creature.";
            throw new NotImplementedException();
        }
    }
}
