//SevertheBloodline
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class SevertheBloodline : Sorcery 
    {
        public SevertheBloodline() : base("Sorcery, 3B (4)")
        {
            Name = "Sever the Bloodline";
            Current.Text = @"Exile target creature and all other creatures with the same name as that creature. Flashback {5}{B}{B} (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
