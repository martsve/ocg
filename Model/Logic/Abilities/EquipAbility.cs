using System;
using Delver.Costs;

namespace Delver.AbilitiesSpace
{
    [Serializable]
    internal class EquipAbility : Ability
    {
        public EquipAbility(string cost)
            : base(AbiltiyType.Activated)
        {
            costs.Add(new OnlyAsSorceryCost());
            costs.Add(new PayManaCost(cost));
            effects.Add(new EquipEffect());
        }
    }    
}