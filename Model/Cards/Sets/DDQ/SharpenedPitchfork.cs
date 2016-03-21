//SharpenedPitchfork
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class SharpenedPitchfork : Equipment 
    {
        public SharpenedPitchfork() : base("2", "1")
        {
            Name = "Sharpened Pitchfork";
            Base.Text = @"Equipped creature has first strike. As long as equipped creature is a Human, it gets +1/+1. Equip {1}";
            NotImplemented();
        }
    }
}
