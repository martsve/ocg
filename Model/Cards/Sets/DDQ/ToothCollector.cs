//ToothCollector
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class ToothCollector : Creature 
    {
        public ToothCollector() : base("2B", 3, 2)
        {
            Name = "Tooth Collector";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Rogue");
            Base.Text = @"When Tooth Collector enters the battlefield, target creature an opponent controls gets -1/-1 until end of turn. Delirium � At the beginning of each opponent's upkeep, if there are four or more card types among cards in your graveyard, target creature that player controls gets -1/-1 until end of turn.";
            throw new NotImplementedException();
        }
    }
}
