﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class ChampionoftheParish : Creature
    {
        public ChampionoftheParish() : base("W", 1, 1)
        {
            Name = "Champion of the Parish";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Cleric");

            Base.When(
                 $"Whenever another Human enters the battlefield under your control, put a +1/+1 counter on {this}.",
                 EventCollection.CreatureEnterTheBattlefield(filter),
                 PutCounterOnCreature
            );
        }

        public bool filter(BaseEventInfo e)
        {
            return e.triggerCard != e.sourceCard && e.triggerCard.Current.Subtype.Contains("Human");
        }

        public void PutCounterOnCreature(BaseEventInfo e)
        {
            if (filter(e))
                e.Game.Methods.AddCounter(this, new PlussCounter());
        }
    }
}
