//HavengulRunebinder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class HavengulRunebinder : Creature 
    {
        public HavengulRunebinder() : base("Creature � Human Wizard 2/2, 2UU (4)")
        {
            Name = "Havengul Runebinder";
            Current.Text = @"{2}{U}, {T}, Exile a creature card from your graveyard: Put a 2/2 black Zombie creature token onto the battlefield, then put a +1/+1 counter on each Zombie creature you control.";
            throw new NotImplementedException();
        }
    }
}
