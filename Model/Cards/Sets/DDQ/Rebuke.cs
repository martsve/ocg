//Rebuke
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class Rebuke : Instant 
    {
        public Rebuke() : base("2W")
        {
            Name = "Rebuke";
            Base.Text = @"Destroy target attacking creature.";
            NotImplemented();
        }
    }
}
