//SlayeroftheWicked
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class SlayeroftheWicked : Creature 
    {
        public SlayeroftheWicked() : base("Creature � Human Soldier 3/2, 3W (4)")
        {
            Name = "Slayer of the Wicked";
            Current.Text = @"When Slayer of the Wicked enters the battlefield, you may destroy target Vampire, Werewolf, or Zombie.";
            throw new NotImplementedException();
        }
    }
}
