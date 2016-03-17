//VoiceoftheProvinces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class VoiceoftheProvinces : Creature 
    {
        public VoiceoftheProvinces() : base("Creature � Angel 3/3, 4WW (6)")
        {
            Name = "Voice of the Provinces";
            Current.Text = @"Flying When Voice of the Provinces enters the battlefield, put a 1/1 white Human creature token onto the battlefield.";
            throw new NotImplementedException();
        }
    }
}
