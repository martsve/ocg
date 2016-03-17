//ToothCollector
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ToothCollector : Creature 
    {
        public ToothCollector() : base("Creature � Human Rogue 3/2, 2B (3)")
        {
            Name = "Tooth Collector";
            Current.Text = @"When Tooth Collector enters the battlefield, target creature an opponent controls gets -1/-1 until end of turn. Delirium � At the beginning of each opponent's upkeep, if there are four or more card types among cards in your graveyard, target creature that player controls gets -1/-1 until end of turn.";
            throw new NotImplementedException();
        }
    }
}
