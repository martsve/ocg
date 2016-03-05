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
    internal class EmancipationAngel : Creature
    {
        public EmancipationAngel() : base("1WW", 3, 3)
        {
            Name = "Emancipation Angel";
            Subtype.Add("Angel");
            AddKeyword(Keywords.Flying);

            Events.Add(new Events.ThisEnterTheBattlefield(new CallbackEffect(ThisEnters))
            {
                Text = $"When {this} enters the battlefield, return a permanent you control to its owner's hand."
            });
        }

        public void ThisEnters(BaseEventInfo e)
        {
            var list = e.triggerPlayer.Battlefield.Where(x=>x.isType(CardType.Creature)).ToList();
            if (list.Count() > 0)
            {
                Card card = null;
                while (card == null)
                    card = e.triggerPlayer.request.RequestFromObjects(RequestType.SelectTarget, $"Select permanent to return to owner's hand", list);
                e.Game.Methods.ChangeZone(card, card.Zone, Zone.Hand);
            }
        }
    }
}
