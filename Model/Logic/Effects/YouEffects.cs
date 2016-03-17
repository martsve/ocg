using System;

namespace Delver
{
    [Serializable]
    internal class GainLifeEffect : Effect
    {
        private readonly int life;

        public GainLifeEffect(int life)
        {
            this.life = life;
            Text = $"You gain {life} life";
        }

        public override void Invoke(EventInfo e)
        {
            e.Context.Methods.GainLife(e.sourcePlayer, e.sourceCard, life);
        }
    }
}