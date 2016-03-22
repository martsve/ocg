//SeraphSanctuary
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
    class SeraphSanctuary : Land 
    {
        public SeraphSanctuary() : base()
        {
            Name = "Seraph Sanctuary";

            Base.When(
                 $"When Seraph Sanctuary enters the battlefield, you gain 1 life.",
                 EventCollection.ThisEnterTheBattlefield(),
                 e => e.Context.Methods.GainLife(e.SourcePlayer, e.SourceCard, 1)
             );

            Base.When(
                 $"Whenever an Angel enters the battlefield under your control, you gain 1 life.",
                 EventCollection.CreatureEnterTheBattlefield(e => e.TriggerCard.IsSubType("Angel")),
                 e => e.Context.Methods.GainLife(e.SourcePlayer, e.SourceCard, 1)
             );

            Base.CardAbilities.Add(new BasicLandAbility(Identity.Colorless));
        }
    }
}
