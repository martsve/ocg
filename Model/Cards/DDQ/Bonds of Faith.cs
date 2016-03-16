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
    internal class BondsofFaith : Aura
    {
        public BondsofFaith() : base("2W", new Target.Creature())
        {
            Name = "Bonds of Faith";
            Base.Text = "Enchanted creature gets +2/+2 as long as it's a Human. Otherwise, it can't attack or block.";

            Base.Following(GivePluss, LayerType.PowerChanging_C_Modify);
            Base.Following(CantAttack, LayerType.AbilityAdding);
        }

        public void GivePluss(BaseEventInfo e)
        {
            var card = e.Enchanted.Card;
            if (card.Current.Subtype.Contains("Human"))
            {
                card.Current.Power += 2;
                card.Current.Thoughness += 2;
            }
        }

        public void CantAttack(BaseEventInfo e)
        {
            var card = e.Enchanted.Card;
            if (!card.Current.Subtype.Contains("Human"))
                    card.Current.CanAttack = false;
                card.Current.CanBlock = false;
        }
    }

}
