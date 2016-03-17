//IncreasingDevotion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class IncreasingDevotion : Sorcery 
    {
        public IncreasingDevotion() : base("3WW")
        {
            Name = "Increasing Devotion";
            Base.Text = @"Put five 1/1 white Human creature tokens onto the battlefield. If Increasing Devotion was cast from a graveyard, put ten of those tokens onto the battlefield instead. Flashback {7}{W}{W} (You may cast this card from your graveyard for its flashback cost. Then exile it.)";
            throw new NotImplementedException();
        }
    }
}
