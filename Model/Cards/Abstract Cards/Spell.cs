using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    internal interface IStackCard
    {
        Player Controller { get; set; }
        void Resolve(Context Context);
    }

    [Serializable]
    internal class Spell : Card, IStackCard
    {
        public Spell(CardType type) : base(type)
        {
        }

        public void Resolve(Context Context)
        {
            Context.Logic.Resolve(this);
        }
    }

    internal interface IImaginaryCard
    {
        Card Source { get; set; }
    }

}
