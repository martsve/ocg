using System;
using Delver.Costs;

namespace Delver.AbilitiesSpace
{
    [Serializable]
    internal class BasicLandAbility : Ability
    {
        public BasicLandAbility(Identity color)
            : base(AbiltiyType.Activated)
        {
            costs.Add(new TapCost());
            effects.Add(new AddManaEffect(color));
            IsManaSource = true;
        }
    }
}