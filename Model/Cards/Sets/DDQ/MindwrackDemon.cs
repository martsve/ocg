//MindwrackDemon
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MindwrackDemon : Creature 
    {
        public MindwrackDemon() : base("2BB", 4, 5)
        {
            Name = "Mindwrack Demon";
            Base.Subtype.Add("Demon");
            Base.Text = @"Flying, trample When Mindwrack Demon enters the battlefield, put the top four cards of your library into your graveyard. Delirium � At the beginning of your upkeep, you lose 4 life unless there are four or more card types among cards in your graveyard.";
            throw new NotImplementedException();
        }
    }
}
