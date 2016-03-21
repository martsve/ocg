//DriveroftheDead
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class DriveroftheDead : Creature 
    {
        public DriveroftheDead() : base("3B", 3, 2)
        {
            Name = "Driver of the Dead";
            Base.Subtype.Add("Vampire");
            Base.Text = @"When Driver of the Dead dies, return target creature card with converted mana cost 2 or less from your graveyard to the battlefield.";
            //throw new NotImplementedException();
        }
    }
}
