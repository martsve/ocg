//SharpenedPitchfork
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class SharpenedPitchfork : Artifact 
    {
        public SharpenedPitchfork() : base("Artifact � Equipment, 2 (2)")
        {
            Name = "Sharpened Pitchfork";
            Current.Text = @"Equipped creature has first strike. As long as equipped creature is a Human, it gets +1/+1. Equip {1}";
            throw new NotImplementedException();
        }
    }
}
