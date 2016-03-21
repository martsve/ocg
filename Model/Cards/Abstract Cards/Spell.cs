using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    [Serializable]
    internal class Spell : Card
    {
        public Spell(CardType type) : base(type)
        {
        }

        public void Resolve(Context Context)
        {
            Context.Logic.Resolve(this);
        }

        public EventInfo BaseEventInfo { get; set; }

    }
}
