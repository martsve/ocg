using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Effects;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class ChampionoftheParish : Creature
    {
        public ChampionoftheParish() : base("W", 1, 1)
        {
            Name = "Champion of the Parish";
            Subtype.Add("Human");
            Subtype.Add("Cleric");

            Events.Add(new Events.CreatureEnterTheBattlefield(new CallbackEffect(CreatureEnter))
            {
                Text = $"Whenever another Human enters the battlefield under your control, put a +1/+1 counter on {this}."
            });
        }

        public void CreatureEnter(BaseEventInfo e)
        {
            if (e.triggerCard.Subtype.Contains("Human"))
                e.Game.Methods.AddCounter(this, new PlussCounter());
        }
    }
}
