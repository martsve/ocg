using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    [Serializable]
    internal class Instant : Spell
    {
        public Instant(string castingCost) : base(CardType.Instant)
        {
            Base.SetCastingCost(castingCost);
        }
    }

}
