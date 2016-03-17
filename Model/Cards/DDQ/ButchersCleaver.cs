//ButchersCleaver
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ButchersCleaver : Artifact 
    {
        public ButchersCleaver() : base("Artifact � Equipment, 3 (3)")
        {
            Name = "Butcher's Cleaver";
            Current.Text = @"Equipped creature gets +3/+0. As long as equipped creature is a Human, it has lifelink. Equip {3}";
            throw new NotImplementedException();
        }
    }
}
