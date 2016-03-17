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
    internal class EmancipationAngel : Creature
    {
        public EmancipationAngel() : base("1WW", 3, 3)
        {
            Name = "Emancipation Angel";
            Base.Subtype.Add("Angel");
            Base.AddKeyword(Keywords.Flying);

            Base.When(
                $"When {this} enters the battlefield, return a permanent you control to its owner's hand.",
                EventCollection.ThisEnterTheBattlefield(),
                ReturnACreature
            );
        }

        public void ReturnACreature(EventInfo e)
        {
            var list = e.triggerPlayer.Battlefield.Where(x=>x.isCardType(CardType.Creature)).ToList();
            if (list.Count() > 0)
            {
                Card card = list.Count() == 1 ? list.First() : null;
                while (card == null)
                    card = e.triggerPlayer.request.RequestFromObjects(RequestType.SelectTarget, $"Select permanent to return to owner's hand", list);
                e.Game.Methods.ChangeZone(card, card.Zone, Zone.Hand);
            }
        }
    }
}
