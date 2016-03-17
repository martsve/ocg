//ForbiddenAlchemy
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ForbiddenAlchemy : Instant 
    {
        public ForbiddenAlchemy() : base("Instant, 2U (3)")
        {
            Name = "Forbidden Alchemy";
            Current.Text = @"Look at the top four cards of your library. Put one of them into your hand and the rest into your graveyard. Flashback {6}{B} (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
