using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;
using Delver;

//namespace Delver.Cards.DDQ

namespace Delver.Cards
{
    [Serializable]
    internal class DoomedTraveler : Creature
    {
        public DoomedTraveler() : base("W", 1, 1)
        {
            Name = "Doomed Traveler";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Soldier");

            Base.When(
                $"When {this} dies, put a 1/1 white Spirit creature token with flying onto the battlefield.",
                EventCollection.ThisDies(),
                PutSpiritTokenIntoPlay
            );

        }
        public void PutSpiritTokenIntoPlay(EventInfo e)
        {
            e.AddToken(new SpiritToken(1, 1));
        }
    }
}
