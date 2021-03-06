﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver;
using Delver.GameSteps;

//namespace Delver.Cards.DDQ
namespace Delver.Cards
{
    [Serializable]
    internal class DearlyDeparted : Creature
    {
        public DearlyDeparted() : base("4WW", 5, 5)
        {
            // TODO Replacement effect
            Name = "Dearly Departed";
            Base.Subtype.Add("Spirit");
            Base.AddKeyword(Keywords.Flying);

            Base.When(
                $"As long as {this} is in your graveyard, each Human creature you control enters the battlefield with an additional +1/+1 counter on it",
                EventCollection.CreatureEnterTheBattlefield(null, null, Zone.Graveyard),
                AddCounterToCreature
            );
        }

        public void AddCounterToCreature(EventInfo e)
        {
            if (e.TriggerCard.Current.Subtype.Contains("Human") && this.Zone == Zone.Graveyard)
                e.Context.Methods.AddCounter(e.TriggerCard, new PlussCounter());
        }
    }
}
