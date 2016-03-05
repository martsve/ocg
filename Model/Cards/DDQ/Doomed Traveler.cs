using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;
using Delver.Effects;
//namespace Delver.Cards.DDQ

namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class DoomedTraveler : Creature
    {
        public DoomedTraveler() : base("W", 1, 1)
        {
            Name = "Doomed Traveler";
            Subtype.Add("Human");
            Subtype.Add("Soldier");

            Events.Add(new Events.ThisDies(new CallbackEffect(ThisDies))
            {
                Text = $"When {this} dies, put a 1/1 white Spirit creature token with flying onto the battlefield."
            });
        }
        public void ThisDies(BaseEventInfo e)
        {
            var token = new SpiritToken(1, 1);
            e.Game.Methods.AddToken(e.triggerPlayer, token);
        }
    }
}
