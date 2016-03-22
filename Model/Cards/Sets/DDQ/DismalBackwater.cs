//DismalBackwater
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
    class DismalBackwater : Land 
    {
        public DismalBackwater() : base()
        {
            Name = "Dismal Backwater";

            NotImplemented();
            Base.Text = @"Dismal Backwater enters the battlefield tapped.";

            Base.When(
                 $"When Dismal Backwater enters the battlefield, you gain 1 life.",
                 EventCollection.ThisEnterTheBattlefield(),
                 e => e.Context.Methods.GainLife(this.Controller, this, 1)
            );

            Base.CardAbilities.Add(new BasicLandAbility(Identity.Black));
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Blue));
        }
    }
}
