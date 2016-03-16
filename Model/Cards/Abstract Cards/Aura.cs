using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    // TODO all card can become/stop being enchantments...
    [Serializable]
    internal abstract class Aura : Enchantment
    {
        protected Aura(string cost, ITarget target) : base(cost)
        {
            Base.Subtype.Add("Aura");
            Base.Effect(
                new AuraEffect(this),
                target
            );
        }
    }

}
