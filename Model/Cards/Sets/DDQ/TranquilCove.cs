//TranquilCove
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
using Delver.AbilitiesSpace;

namespace Delver.Cards
{
    [Serializable]
    class TranquilCove : Land 
    {
        public TranquilCove() : base()
        {
            Name = "Tranquil Cove";

            NotImplemented();
            Base.Text = @"Tranquil Cove enters the battlefield tapped.";

            Base.When(
                 $"When Tranquil Cove enters the battlefield, you gain 1 life.",
                 EventCollection.ThisEnterTheBattlefield(),
                 e => e.Context.Methods.GainLife(this.Controller, this, 1)
            );

            Base.CardAbilities.Add(new BasicLandAbility(Identity.White));
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Blue));
        }
    }
}

