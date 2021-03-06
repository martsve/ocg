//VillageBellRinger
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class VillageBellRinger : Creature 
    {
        public VillageBellRinger() : base("2W", 1, 4)
        {
            Name = "Village Bell-Ringer";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Scout");
            Base.AddKeyword(Keywords.Flash);
        }
    }
}
