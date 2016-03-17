//VillageBellRinger
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class VillageBellRinger : Creature 
    {
        public VillageBellRinger() : base("Creature � Human Scout 1/4, 2W (3)")
        {
            Name = "Village Bell-Ringer";
            Current.Text = @"Flash (You may cast this spell any time you could cast an instant.)";
            throw new NotImplementedException();
        }
    }
}
