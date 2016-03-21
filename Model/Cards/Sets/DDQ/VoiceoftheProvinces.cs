//VoiceoftheProvinces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class VoiceoftheProvinces : Creature 
    {
        public VoiceoftheProvinces() : base("4WW", 3, 3)
        {
            Name = "Voice of the Provinces";
            Base.Subtype.Add("Angel");
            Base.Text = @"Flying When Voice of the Provinces enters the battlefield, put a 1/1 white Human creature token onto the battlefield.";
            //throw new NotImplementedException();
        }
    }
}
