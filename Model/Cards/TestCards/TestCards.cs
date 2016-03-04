using System;
using Delver.AbilitiesSpace;

namespace Delver.Cards.TestCards
{

    #region Creatures

    [Serializable]
    internal class Bear : Creature
    {
        public Bear() : base("1G", 2, 2)
        {
            Subtype.Add("bear");
            Events.Add(new Events.ThisEnterTheBattlefield(ThisEnters)
            {
                Text = "When this enters play, creatures you control get +1/+0 until end of turn"
            });
        }

        public void ThisEnters(BaseEventInfo e)
        {
            e.Game.LayeredEffects.Add(new LayerEffects.AlterPlayersCreaturesStats(e.sourceCard.Controller, 1, 0, Duration.EndOfTurn));
            e.Game.Methods.AddCounter(this, new PlussCounter());
        }
    }

    #endregion

    #region Lands

    [Serializable]
    internal class Plains : Land
    {
        public Plains() : base(CardType.Basic)
        {
            BasicType.Add(Identity.White);
            Abilities.Add(new BasicLandAbility(Identity.White));
        }
    }

    [Serializable]
    internal class Island : Land
    {
        public Island() : base(CardType.Basic)
        {
            BasicType.Add(Identity.Blue);
            Abilities.Add(new BasicLandAbility(Identity.Blue));
        }
    }

    [Serializable]
    internal class Swamp : Land
    {
        public Swamp() : base(CardType.Basic)
        {
            BasicType.Add(Identity.Black);
            Abilities.Add(new BasicLandAbility(Identity.Black));
        }
    }

    [Serializable]
    internal class Mountain : Land
    {
        public Mountain() : base(CardType.Basic)
        {
            BasicType.Add(Identity.Red);
            Abilities.Add(new BasicLandAbility(Identity.Red));
        }
    }


    [Serializable]
    internal class Forest : Land
    {
        public Forest() : base(CardType.Basic)
        {
            BasicType.Add(Identity.Green);
            Abilities.Add(new BasicLandAbility(Identity.Green));
        }
    }

    #endregion

    #region Spells

    [Serializable]
    internal class LightningStrike : Spell
    {
        public LightningStrike() : base(CardType.Instant)
        {
            Name = "Lightning Strike";
            SetCastingCost("1G");
            Abilities.Add(new Effects.DealDamageEffect(new Target.CreatureOrPlayer(), 3)
            {
                Text = "Deal 3 damage to target creature or player"
            });
        }
    }

    [Serializable]
    internal class Flicker : Spell
    {
        public Flicker() : base(CardType.Instant)
        {
            Name = "Flicker";
            SetCastingCost("1");
            Abilities.Add(new Effects.FlickerEffect(new Target.Creature())
            {
                Text = "Exile target creature. Return it to the battlefield tapped."
            });
        }
    }


    [Serializable]
    internal class DrainLife : Spell
    {
        public DrainLife() : base(CardType.Sorcery)
        {
            Name = "Drain Life";

            SetCastingCost("1B");
            Abilities.Add(new Effects.LoseLifeEffect(new Target.Player(), 1));
            Abilities.Add(new Effects.GainLifeEffect(1));
        }
    }

    [Serializable]
    internal class StoneRain : Spell
    {
        public StoneRain() : base(CardType.Sorcery)
        {
            Name = "Stone Rain";

            SetCastingCost("1BB");
            Abilities.Add(new Effects.DestroyTargetLandEffect());
        }
    }

    #endregion
}