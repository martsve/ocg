//MakeshiftMauler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class MakeshiftMauler : Creature 
    {
        public MakeshiftMauler() : base("Creature � Zombie Horror 4/5, 3U (4)")
        {
            Name = "Makeshift Mauler";
            Current.Text = @"As an additional cost to cast Makeshift Mauler, exile a creature card from your graveyard.";
            throw new NotImplementedException();
        }
    }
}
