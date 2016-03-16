using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    [Serializable]
    internal class Sorcery : Spell
    {
        public Sorcery(string castingCost) : base(CardType.Sorcery)
        {
            Base.SetCastingCost(castingCost);
        }
    }

}
