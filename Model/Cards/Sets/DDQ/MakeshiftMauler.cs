//MakeshiftMauler
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MakeshiftMauler : Creature 
    {
        public MakeshiftMauler() : base("3U", 4, 5)
        {
            Name = "Makeshift Mauler";
            Base.Subtype.Add("Zombie");
            Base.Subtype.Add("Horror");
            Base.Text = @"As an additional cost to cast Makeshift Mauler, exile a creature card from your graveyard.";
            throw new NotImplementedException();
        }
    }
}
