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

        public override void Invoke(EventInfo e)
        {
            foreach (Card target in e.Targets)
            {
                e.Context.Methods.ChangeZone(target, Zone.Battlefield, Zone.Exile);
                e.Context.Methods.ChangeZone(target, Zone.Exile, Zone.Battlefield);
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

        public override void Invoke(EventInfo e)
        {
            foreach (Player target in e.Targets)
                e.Context.Methods.LoseLife(target, e.SourceCard, life);
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

        public override void Invoke(EventInfo e)
        {
            foreach (var obj in e.Targets)
                e.Context.Methods.DealDamage(e.SourceCard, obj, damage);
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

        public override void Invoke(EventInfo e)
        {
            foreach (Card card in e.Targets)
                e.Context.Methods.Destroy(e.SourceCard, card);
        }
    }
}