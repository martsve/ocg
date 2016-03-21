//GryffVanguard
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class GryffVanguard : Creature 
    {
        public GryffVanguard() : base("4U", 3, 2)
        {
            Name = "Gryff Vanguard";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Knight");
            Base.Text = @"Flying When Gryff Vanguard enters the battlefield, draw a card.";
            NotImplemented();
        }
    }
}
