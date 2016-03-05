﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class CathedralSanctifier : Creature
    {
        public CathedralSanctifier() : base("W", 1, 1)
        {
            Name = "Cathedral Sanctifier";
            Subtype.Add("Human");
            Subtype.Add("Cleric");

            Events.Add(new Events.ThisEnterTheBattlefield(x=>x.Game.Methods.GainLife(x.triggerPlayer, x.triggerCard, 3))
            {
                Text = $"When {this} enters the battlefield, you gain 3 life."
            });
        }
    }
}
