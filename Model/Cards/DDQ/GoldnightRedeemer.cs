//GoldnightRedeemer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class GoldnightRedeemer : Creature 
    {
        public GoldnightRedeemer() : base("Creature � Angel 4/4, 4WW (6)")
        {
            Name = "Goldnight Redeemer";
            Current.Text = @"Flying When Goldnight Redeemer enters the battlefield, you gain 2 life for each other creature you control.";
            throw new NotImplementedException();
        }
    }
}
