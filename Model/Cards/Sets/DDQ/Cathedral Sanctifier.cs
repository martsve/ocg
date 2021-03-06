﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver;

//namespace Delver.Cards.DDQ
namespace Delver.Cards
{
    [Serializable]
    internal class CathedralSanctifier : Creature
    {
        public CathedralSanctifier() : base("W", 1, 1)
        {
            Name = "Cathedral Sanctifier";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Cleric");

            Base.When(
                 $"When {this} enters the battlefield, you gain 3 life.",
                 EventCollection.ThisEnterTheBattlefield(),
                 new GainLifeEffect(3)
            );
        }
    }
}
