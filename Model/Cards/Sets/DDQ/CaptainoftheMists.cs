//CaptainoftheMists
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class CaptainoftheMists : Creature 
    {
        public CaptainoftheMists() : base("2U", 2, 3)
        {
            Name = "Captain of the Mists";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Wizard");
            Base.Text = @" {1}{U}, {T}: You may tap or untap target permanent.";

            Base.When(
                 $"Whenever another Human enters the battlefield under your control, untap Captain of the Mists.",
                 EventCollection.CreatureEnterTheBattlefield(x => x.TriggerCard.IsSubType("Human")),
                 e => e.Context.Methods.Untap(this)
            );

            NotImplemented();
        }
        
    }
}
