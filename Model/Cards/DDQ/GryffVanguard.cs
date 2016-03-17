//GryffVanguard
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class GryffVanguard : Creature 
    {
        public GryffVanguard() : base("Creature � Human Knight 3/2, 4U (5)")
        {
            Name = "Gryff Vanguard";
            Current.Text = @"Flying When Gryff Vanguard enters the battlefield, draw a card.";
            throw new NotImplementedException();
        }
    }
}
