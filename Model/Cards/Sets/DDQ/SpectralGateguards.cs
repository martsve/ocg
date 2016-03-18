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
            Base.Text = @"Soulbond (You may pair this creature with another unpaired creature when either enters the battlefield. They remain paired for as long as you control both of them.)";
            throw new NotImplementedException();
        }
    }
}
