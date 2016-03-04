using System;
using Delver.Costs;
using Delver.Effects;

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