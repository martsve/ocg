using System;
using Delver.AbilitiesSpace;
using Delver.Effects;

namespace Delver.Cards.TestCards
{

    #region Creatures

    [Serializable]
    internal class Bear : Creature
    {
        public Bear() : base("1G", 2, 2)
        {
            Subtype.Add("bear");
            Events.Add(new Events.ThisEnterTheBattlefield(new CallbackEffect(ThisEnters))
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
    internal class LightningStrike : Instant
    {
        public LightningStrike() : base("1G")
        {
            Name = "Lightning Strike";
            Abilities.Add(new Effects.DealDamageEffect(new Target.CreatureOrPlayer(), 3)
            {
                Text = "Deal 3 damage to target creature or player"
            });
        }
    }

    [Serializable]
    internal class Flicker : Instant
    {
        public Flicker() : base("1")
        {
            Name = "Flicker";
            Abilities.Add(new Effects.FlickerEffect(new Target.Creature())
            {
                Text = "Exile target creature. Return it to the battlefield tapped."
            });
        }
    }


    [Serializable]
    internal class DrainLife : Sorcery
    {
        public DrainLife() : base("1B")
        {
            Name = "Drain Life";
            Abilities.Add(new Effects.LoseLifeEffect(new Target.Player(), 1));
            Abilities.Add(new Effects.GainLifeEffect(1));
        }
    }

    [Serializable]
    internal class StoneRain : Sorcery
    {
        public StoneRain() : base("1BB")
        {
            Name = "Stone Rain";
            Abilities.Add(new Effects.DestroyTargetLandEffect());
        }
    }

    #endregion
}