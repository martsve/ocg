//GoldnightRedeemer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class GoldnightRedeemer : Creature 
    {
        public GoldnightRedeemer() : base("4WW", 4, 4)
        {
            Name = "Goldnight Redeemer";
            Base.Subtype.Add("Angel");
            Base.Text = @"Flying When Goldnight Redeemer enters the battlefield, you gain 2 life for each other creature you control.";
            throw new NotImplementedException();
        }
    }
}
