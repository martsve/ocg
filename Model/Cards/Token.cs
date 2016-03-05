using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver.Tokens
{
    [Serializable]
    internal class AngelToken : CreatureToken
    {
        public AngelToken(int power, int thoughness) : base(power, thoughness)
        {
            Name = "Angel";
            Subtype.Add("Angel");
            AddKeyword(Keywords.Flying);
        }
    }


    [Serializable]
    internal class SpiritToken : CreatureToken
    {
        public SpiritToken(int power, int thoughness) : base(power, thoughness)
        {
            Name = "Spirit";
            Subtype.Add("Spirit");
            AddKeyword(Keywords.Flying);
        }
    }

    [Serializable]
    internal class HumanToken : CreatureToken
    {
        public HumanToken(int power, int thoughness) : base(power, thoughness)
        {
            Name = "Human";
            Subtype.Add("Human");
        }
    }

}
