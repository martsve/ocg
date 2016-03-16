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
            Base.Subtype.Add("bear");
            Base.When(
                $"When {this} enters play, creatures you control get +1/+0 until end of turn",
                EventCollection.ThisEnterTheBattlefield(),
                CreaturesGetPluss
            );
        }

        public void CreaturesGetPluss(BaseEventInfo e)
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
            Base.CardAbilities.Add(new BasicLandAbility(Identity.White));
        }
    }

    [Serializable]
    internal class Island : Land
    {
        public Island() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Blue));
        }
    }

    [Serializable]
    internal class Swamp : Land
    {
        public Swamp() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Black));
        }
    }

    [Serializable]
    internal class Mountain : Land
    {
        public Mountain() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Red));
        }
    }


    [Serializable]
    internal class Forest : Land
    {
        public Forest() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Green));
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

            Base.Effect(
                $"Deal 3 damage to target creature or player",
                new DealDamageEffect(3),
                new Target.CreatureOrPlayer()
            );

        }
    }

    [Serializable]
    internal class Flicker : Instant
    {
        public Flicker() : base("1")
        {
            Name = "Flicker";
            Base.Effect(
                "Exile target creature. Return it to the battlefield tapped.",
                new FlickerEffect(),
                new Target.Creature()
            );
        }
    }


    [Serializable]
    internal class DrainLife : Sorcery
    {
        public DrainLife() : base("1B")
        {
            Name = "Drain Life";
            Base.Effect(new LoseLifeEffect(1), new Target.Player());
            Base.Effect(new GainLifeEffect(1));
        }
    }

    [Serializable]
    internal class StoneRain : Sorcery
    {
        public StoneRain() : base("1BB")
        {
            Name = "Stone Rain";
            Base.Effect(new DestroyTargetLandEffect());
        }
    }

    #endregion
}