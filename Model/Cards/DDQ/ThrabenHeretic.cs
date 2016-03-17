//ThrabenHeretic
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class ThrabenHeretic : Creature 
    {
        public ThrabenHeretic() : base("1W", 2, 2)
        {
            Name = "Thraben Heretic";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Wizard");
            Base.Text = @"{T}: Exile target creature card from a graveyard.";
            throw new NotImplementedException();
        }
    }
}
