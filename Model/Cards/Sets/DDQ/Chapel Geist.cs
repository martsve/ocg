﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;

//namespace Delver.Cards.DDQ
namespace Delver.Cards
{
    [Serializable]
    internal class ChapelGeist : Creature
    {
        public ChapelGeist() : base("1WW", 2, 3)
        {
            Name = "Chapel Geist";
            Base.Subtype.Add("Spirit");
        }
    }
}
