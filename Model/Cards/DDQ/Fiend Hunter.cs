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
    internal class FiendHunter : Creature
    {

        GameObjectReferance exiledCard; 

        public FiendHunter() : base("1WW", 1, 3)
        {
            Name = "Fiend Hunter";
            Subtype.Add("Human");
            Subtype.Add("Cleric");

            Events.Add(
                new Events.ThisEnterTheBattlefield(
                    new TargetedCallbackEffect(ExileAnother,
                    new List<ITarget>() { new Target.Creature(x => x != this) }
                )
                ) {
                    Text = $"When Fiend Hunter enters the battlefield, you may exile another target creature."
                }
            );

            Events.Add(
                new Events.ThisLeavesTheBattlefield(new CallbackEffect(ReturnExiled))
                {
                    Text = $"When Fiend Hunter enters the battlefield, you may exile another target creature."
                }
            );
        }
        public void ExileAnother(BaseEventInfo e, List<ITarget> targets)
        {
            foreach (Card card in targets)
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
