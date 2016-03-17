using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{

    [Serializable]
    internal abstract class Artifact : Spell
    {
        protected Artifact(string cost) : base(CardType.Artifact | CardType.Permanent)
        {
            Base.SetCastingCost(cost);
        }
    }
}
