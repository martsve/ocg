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
        public ThrabenHeretic() : base("Creature � Human Wizard 2/2, 1W (2)")
        {
            Name = "Thraben Heretic";
            Current.Text = @"{T}: Exile target creature card from a graveyard.";
            throw new NotImplementedException();
        }
    }
}
