//SpectralGateguards
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class SpectralGateguards : Creature 
    {
        public SpectralGateguards() : base("4W", 2, 5)
        {
            Name = "Spectral Gateguards";
            Base.Subtype.Add("Spirit");
            Base.Subtype.Add("Soldier");
            Base.AddKeyword(Keywords.Soulbond);
            Base.Text = @"As long as Spectral Gateguards is paired with another creature, both creatures have vigilance.";
            NotImplemented();
        }
    }
}
