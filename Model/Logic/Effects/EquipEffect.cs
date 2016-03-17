using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{
    [Serializable]
    internal class EquipEffect : Effect
    {
        public EquipEffect()
        {
            AddTarget(new Target.PermanentYouControl(CardType.Creature));
        }

        public override void Invoke(EventInfo e)
        {
            foreach (Card target in e.Targets)
            {
                // Equip to a creature
                throw new NotImplementedException();
            }
        }
    }
}