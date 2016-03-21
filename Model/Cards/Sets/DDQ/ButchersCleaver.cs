//ButchersCleaver
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class ButchersCleaver : Equipment 
    {
        public ButchersCleaver() : base("3", "3")
        {
            Name = "Butcher's Cleaver";
            Base.Text = @"Equipped creature gets +3/+0. As long as equipped creature is a Human, it has lifelink. Equip {3}";
            //throw new NotImplementedException();
        }
    }
}
