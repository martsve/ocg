//SlayeroftheWicked
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class SlayeroftheWicked : Creature 
    {
        public SlayeroftheWicked() : base("3W", 3, 2)
        {
            Name = "Slayer of the Wicked";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Soldier");
            Base.Text = @"When Slayer of the Wicked enters the battlefield, you may destroy target Vampire, Werewolf, or Zombie.";
            throw new NotImplementedException();
        }
    }
}
