using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.AbilitiesSpace;

namespace Delver
{
    // TODO all card can become/stop being enchantments...
    [Serializable]
    internal abstract class Equipment : Artifact
    {
        protected Equipment(string cost, string equipCost) : base(cost)
        {
            Base.Subtype.Add("Equipment");
            Base.CardAbilities.Add(new EquipAbility(equipCost));
        }
    }

}
