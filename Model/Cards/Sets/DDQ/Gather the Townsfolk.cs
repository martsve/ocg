using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;

//namespace Delver.Cards.DDQ
namespace Delver.Cards
{
    [Serializable]
    internal class GathertheTownsfolk : Sorcery
    {
        public GathertheTownsfolk() : base("1W")
        {
            Name = "Gather the Townsfolk";
            Base.Effect(
                "Put two 1/1 white Human creature tokens onto the battlefield.\nFateful hour — If you have 5 or less life, put five of those tokens onto the battlefield instead.",
                new PutTokensEffect()
            );

        }
    }

    [Serializable]
    internal class PutTokensEffect : Effect
    {
        public override void Invoke(EventInfo e) 
        {
            int N = e.sourcePlayer.Life <= 5 ? 5 : 2;

            for (int i = 0; i < N; i++)
                e.Game.Methods.AddToken(e.sourcePlayer, new HumanToken(1,1));
        }
    }
}
