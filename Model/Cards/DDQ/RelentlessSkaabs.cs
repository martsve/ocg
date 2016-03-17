//RelentlessSkaabs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class RelentlessSkaabs : Creature 
    {
        public RelentlessSkaabs() : base("Creature � Zombie 4/4, 3UU (5)")
        {
            Name = "Relentless Skaabs";
            Current.Text = @"As an additional cost to cast Relentless Skaabs, exile a creature card from your graveyard. Undying (When this creature dies, if it had no +1/+1 counters on it, return it to the battlefield under its owner's control with a +1/+1 counter on it.)";
            throw new NotImplementedException();
        }
    }
}
