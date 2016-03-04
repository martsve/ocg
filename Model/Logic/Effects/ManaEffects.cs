using System;

namespace Delver.Effects
{
    [Serializable]
    internal class AddManaEffect : Effect
    {
        private readonly Identity _produces;

        public AddManaEffect(Identity color)
        {
            _produces = color;
            var m = new Mana(_produces);
            Text = $"Add {m} to your manapool";
        }

        public override void Invoke(BaseEventInfo e)
        {
            e.Game.Methods.AddMana(e.sourcePlayer, e.sourceCard, _produces);
        }
    }
}