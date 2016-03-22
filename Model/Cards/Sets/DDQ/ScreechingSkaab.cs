//ScreechingSkaab
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class ScreechingSkaab : Creature 
    {
        public ScreechingSkaab() : base("1U", 2, 1)
        {
            Name = "Screeching Skaab";
            Base.Subtype.Add("Zombie");
            Base.Text = @"";

            Base.When(
                 $"When Screeching Skaab enters the battlefield, put the top two cards of your library into your graveyard.",
                 EventCollection.ThisEnterTheBattlefield(),
                 e => e.Context.Methods.Mill(e.SourcePlayer, e.SourceCard, 2)
             );
        }
    }
}
