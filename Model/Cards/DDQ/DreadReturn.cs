//DreadReturn
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class DreadReturn : Sorcery 
    {
        public DreadReturn() : base("Sorcery, 2BB (4)")
        {
            Name = "Dread Return";
            Current.Text = @"Return target creature card from your graveyard to the battlefield. Flashback�Sacrifice three creatures. (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
