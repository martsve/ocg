using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{

    [Serializable]
    internal abstract class Land : Card
    {
        protected Land() : base(CardType.Land | CardType.Permanent)
        {
        }

        protected Land(CardType cardType) : this()
        {
            Base.AddType(cardType);
        }
    }
}
