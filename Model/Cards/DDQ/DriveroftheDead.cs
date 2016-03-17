//DriveroftheDead
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class DriveroftheDead : Creature 
    {
        public DriveroftheDead() : base("Creature � Vampire 3/2, 3B (4)")
        {
            Name = "Driver of the Dead";
            Current.Text = @"When Driver of the Dead dies, return target creature card with converted mana cost 2 or less from your graveyard to the battlefield.";
            throw new NotImplementedException();
        }
    }
}
