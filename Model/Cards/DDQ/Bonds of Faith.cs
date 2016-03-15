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
        public BondsofFaith() : base("2W")
        {
            Name = "Bonds of Faith";
            Text = "Enchanted creature gets +2/+2 as long as it's a Human. Otherwise, it can't attack or block.";
        }

        public void Invoke(BaseEventInfo e)
        {
            var attachedTo = e.triggerCard;
            if (attachedTo.Subtype.Contains("Human"))
            {
                attachedTo.Power += 2;
                attachedTo.Thoughness += 2;
            }
            else
            {
                attachedTo.CanAttack = false;
                attachedTo.CanBlock = false;
                throw new NotImplementedException("how to restore status?");
            }
        }

    }
}
