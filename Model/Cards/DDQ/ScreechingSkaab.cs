//ScreechingSkaab
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ScreechingSkaab : Creature 
    {
        public ScreechingSkaab() : base("1U", 2, 1)
        {
            Name = "Screeching Skaab";
            Base.Subtype.Add("Zombie");
            Base.Text = @"When Screeching Skaab enters the battlefield, put the top two cards of your library into your graveyard. Its screeching is the sound of you losing your mind.";
            throw new NotImplementedException();
        }
    }
}
