//MoanoftheUnhallowed
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MoanoftheUnhallowed : Sorcery 
    {
        public MoanoftheUnhallowed() : base("2BB")
        {
            Name = "Moan of the Unhallowed";
            Base.Text = @"Put two 2/2 black Zombie creature tokens onto the battlefield. Flashback {5}{B}{B} (You may cast this card from your graveyard for its flashback cost. Then exile it.) For a ghoul, every village is a buffet and every disaster is a reunion.";
            //throw new NotImplementedException();
        }
    }
}
