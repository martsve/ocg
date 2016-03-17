//Topplegeist
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class Topplegeist : Creature 
    {
        public Topplegeist() : base("W", 1, 1)
        {
            Name = "Topplegeist";
            Base.Subtype.Add("Spirit");
            Base.Text = @"Flying When Topplegeist enters the battlefield, tap target creature an opponent controls. Delirium � At the beginning of each opponent's upkeep, if there are four or more card types among cards in your graveyard, tap target creature that player controls.";
            throw new NotImplementedException();
        }
    }
}
