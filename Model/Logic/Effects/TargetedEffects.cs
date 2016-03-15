using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{
    [Serializable]
    internal class FlickerEffect : Effect
    {
        public FlickerEffect(params ITarget[] targets)
        {
            AddTarget(targets);
        }

        public override void Invoke(BaseEventInfo e)
        {
            foreach (Card target in e.Targets)
            {
                e.Game.Methods.ChangeZone(target, Zone.Battlefield, Zone.Exile);
                e.Game.Methods.ChangeZone(target, Zone.Exile, Zone.Battlefield);
            }
        }
    }

    [Serializable]
    internal class LoseLifeEffect : Effect
    {
        private readonly int life;

        public LoseLifeEffect(int life, params ITarget[] targets)
        {
            AddTarget(targets);
            this.life = life;
            Text = $"Target player lose {life} life";
        }

        public override void Invoke(BaseEventInfo e)
        {
            foreach (Player target in e.Targets)
                e.Game.Methods.LoseLife(target, e.sourceCard, life);
        }
    }

    [Serializable]
    internal class DealDamageEffect : Effect
    {
        private readonly int damage;

        public DealDamageEffect(int damage, params ITarget[] targets)
        {
            AddTarget(targets);
            this.damage = damage;
        }

        public override void Invoke(BaseEventInfo e)
        {
            foreach (var obj in e.Targets)
                e.Game.Methods.DealDamage(e.sourceCard, obj, damage);
        }
    }

    [Serializable]
    internal class DestroyTargetLandEffect : Effect
    {
        public DestroyTargetLandEffect()
        {
            AddTarget(new Target.Permanent(CardType.Land));
            Text = $"Destroy target land";
        }

        public override void Invoke(BaseEventInfo e)
        {
            foreach (Card card in e.Targets)
                e.Game.Methods.Destroy(e.sourceCard, card);
        }
    }
}