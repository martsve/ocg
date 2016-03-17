//MoanoftheUnhallowed
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class MoanoftheUnhallowed : Sorcery 
    {
        public MoanoftheUnhallowed() : base("Sorcery, 2BB (4)")
        {
            Name = "Moan of the Unhallowed";
            Current.Text = @"Put two 2/2 black Zombie creature tokens onto the battlefield. Flashback {5}{B}{B} (You may cast this card from your graveyard for its flashback cost. Then exile it.) For a ghoul, every village is a buffet and every disaster is a reunion.";
            throw new NotImplementedException();
        }
    }
}
