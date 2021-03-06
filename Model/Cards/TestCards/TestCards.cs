﻿using System;
using Delver.AbilitiesSpace;

namespace Delver.Cards.Test
{

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

        public void CreaturesGetPluss(EventInfo e)
        {
            e.Context.LayeredEffects.Add(new LayerEffects.AlterPlayersCreaturesStats(e.SourceCard.Controller, 1, 0, Duration.EndOfTurn));
            e.Context.Methods.AddCounter(this, new PlussCounter());
        }
    }


    [Serializable]
    internal class Shock : Instant
    {
        public Shock() : base("W")
        {
            Name = "Shock";

            Base.Effect(
                $"Deal 3 damage to target creature",
                new DealDamageEffect(3),
                new Target.Creature()
            );

        }
    }

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

}