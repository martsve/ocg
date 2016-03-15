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
            Base.Name = "Bonds of Faith";
            Base.Text = "Enchanted creature gets +2/+2 as long as it's a Human. Otherwise, it can't attack or block.";
        }

        public void Invoke(BaseEventInfo e)
        {
            var attachedTo = e.triggerCard;
            if (attachedTo.Current.Subtype.Contains("Human"))
            {
                attachedTo.Current.Power += 2;
                attachedTo.Current.Thoughness += 2;
            }
            else
            {
                attachedTo.Current.CanAttack = false;
                attachedTo.Current.CanBlock = false;
                throw new NotImplementedException("how to restore status?");
            }
        }

    }
}
