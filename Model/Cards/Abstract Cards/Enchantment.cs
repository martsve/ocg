using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{

    [Serializable]
    internal abstract class Enchantment : Spell
    {
        protected Enchantment(string cost) : base(CardType.Enchantment)
        {
            Base.SetCastingCost(cost);
        }
    }
}
