using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{

    [Serializable]
    internal abstract class Creature : Spell
    {
        protected Creature(string cost, int power, int thoughness) : base(CardType.Creature | CardType.Permanent)
        {
            Base.SetCastingCost(cost);
            Base.Power = power;
            Base.Thoughness = thoughness;
        }
    }

    [Serializable]
    internal abstract class CreatureToken : Creature
    {
        public CreatureToken(int power, int thoughness) : base("", power, thoughness)
        {
            Base.Supertype.Add("Token");
            Base.AddCardType(CardType.Token);
        }
    }
}
