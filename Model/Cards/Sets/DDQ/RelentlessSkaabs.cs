//RelentlessSkaabs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class RelentlessSkaabs : Creature 
    {
        public RelentlessSkaabs() : base("3UU", 4, 4)
        {
            Name = "Relentless Skaabs";
            Base.Subtype.Add("Zombie");
            Base.AddKeyword(Keywords.Undying);
            Base.Text = @"As an additional cost to cast Relentless Skaabs, exile a creature card from your graveyard.";
            NotImplemented();
        }
    }
}
