﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver.Tokens
{
    [Serializable]
    internal class AngelToken : CreatureToken
    {
        public AngelToken(Game game, Player player, int power, int thoughness) : base(game, player, power, thoughness)
        {
            Name = "Angel";
            Subtype.Add("Angel");
            AddKeyword(Keywords.Flying);
        }
    }


    [Serializable]
    internal class SpiritToken : CreatureToken
    {
        public SpiritToken(Game game, Player player, int power, int thoughness) : base(game, player, power, thoughness)
        {
            Name = "Spirit";
            Subtype.Add("Spirit");
            AddKeyword(Keywords.Flying);
        }
    }

}