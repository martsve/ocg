//VictimofNight
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class VictimofNight : Instant 
    {
        public VictimofNight() : base("Instant, BB (2)")
        {
            Name = "Victim of Night";
            Current.Text = @"Destroy target non-Vampire, non-Werewolf, non-Zombie creature.";
            throw new NotImplementedException();
        }
    }
}
