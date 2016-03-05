using System;

namespace Delver.Effects
{
    [Serializable]
    internal class FlickerEffect : TargetedEffect
    {
        public FlickerEffect(ITarget target)
        {
            targets.Add(target);
        }

        public override void InvokeWhenValid(BaseEventInfo e)
        {
            var card = TargetCard;
            e.Game.Methods.ChangeZone(card, Zone.Battlefield, Zone.Exile);
            e.Game.Methods.ChangeZone(card, Zone.Exile, Zone.Battlefield);
        }
    }

    [Serializable]
    internal class LoseLifeEffect : TargetedEffect
    {
        private readonly int life;

        public LoseLifeEffect(ITarget target, int life)
        {
            targets.Add(target);
            this.life = life;
            Text = $"Target player lose {life} life";
        }

        public override void InvokeWhenValid(BaseEventInfo e)
        {
            e.Game.Methods.LoseLife(TargetPlayer, e.sourceCard, life);
        }
    }

    [Serializable]
    internal class DealDamageEffect : TargetedEffect
    {
        private readonly int damage;

        public DealDamageEffect(ITarget target, int damage)
        {
            targets.Add(target);
            this.damage = damage;
        }

        public override void InvokeWhenValid(BaseEventInfo e)
        {
            e.Game.Methods.DealDamage(e.sourceCard, Target, damage);
        }
    }

    [Serializable]
    internal class DestroyTargetLandEffect : TargetedEffect
    {
        public DestroyTargetLandEffect()
        {
            targets.Add(new Target.Permanent(CardType.Land));
            Text = $"Destroy target land";
        }

        public override void InvokeWhenValid(BaseEventInfo e)
        {
            e.Game.Methods.Destroy(e.sourceCard, TargetCard);
        }
    }
}