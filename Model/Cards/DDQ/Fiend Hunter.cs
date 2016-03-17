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
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Cleric");

            Base.When(
                $"When Fiend Hunter enters the battlefield, you may exile another target creature.",
                EventCollection.ThisEnterTheBattlefield(),
                ExileAnother,
                new Target.Creature(x => x != this)
            );

            Base.When(
                $"When Fiend Hunter leaves the battlefield, return the exiled creature to the battlefield.",
                EventCollection.ThisLeavesTheBattlefield(),
                ReturnExiled
            );

        }
        public void ExileAnother(EventInfo e)
        {
            foreach (Card card in e.Targets)
            {
                e.Context.Methods.ChangeZone(card, card.Zone, Zone.Exile);
                exiledCard = card.Referance;
            }
        }

        public void ReturnExiled(EventInfo e)
        {
            if (exiledCard?.Card != null)
            {
                e.Context.Methods.ChangeZone(exiledCard.Card, exiledCard.Card.Zone, Zone.Battlefield);
                exiledCard = null;
            }
        }

    }
}
