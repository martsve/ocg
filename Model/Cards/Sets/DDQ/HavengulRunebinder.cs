//HavengulRunebinder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class HavengulRunebinder : Creature 
    {
        public HavengulRunebinder() : base("2UU", 2, 2)
        {
            Name = "Havengul Runebinder";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Wizard");
            Base.Text = @"{2}{U}, {T}, Exile a creature card from your graveyard: Put a 2/2 black Zombie creature token onto the battlefield, then put a +1/+1 counter on each Zombie creature you control.";
            throw new NotImplementedException();
        }
    }
}
