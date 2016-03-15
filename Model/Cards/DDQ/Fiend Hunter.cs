using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;

//namespace Delver.Cards.DDQ

namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class FiendHunter : Creature
    {

        GameObjectReferance exiledCard; 

        public FiendHunter() : base("1WW", 1, 3)
        {
            Name = "Fiend Hunter";
            Subtype.Add("Human");
            Subtype.Add("Cleric");

            When(
                $"When Fiend Hunter enters the battlefield, you may exile another target creature.",
                EventCollection.ThisEnterTheBattlefield(),
                ExileAnother,
                new Target.Creature(x => x != this)
            );

            When(
                $"When Fiend Hunter enters the battlefield, you may exile another target creature.",
                EventCollection.ThisLeavesTheBattlefield(),
                ReturnExiled
            );

        }
        public void ExileAnother(BaseEventInfo e)
        {
            foreach (Card card in e.Targets)
            {
                e.Game.Methods.ChangeZone(card, card.Zone, Zone.Exile);
                exiledCard = card.Referance;
            }
        }

        public void ReturnExiled(BaseEventInfo e)
        {
            if (exiledCard?.Card != null)
            {
                e.Game.Methods.ChangeZone(exiledCard.Card, exiledCard.Card.Zone, Zone.Battlefield);
                exiledCard = null;
            }
        }

    }
}
