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
        void Resolve(Game game);
    }

    [Serializable]
    internal class Spell : Card, IStackCard
    {
        public Spell(CardType type) : base(type)
        {
        }

        public void Resolve(Game game)
        {
            game.Logic.Resolve(this);
        }
    }

    internal interface IImaginaryCard
    {
        Card Source { get; set; }
    }

}
