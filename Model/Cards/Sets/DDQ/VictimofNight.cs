//VictimofNight
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class VictimofNight : Instant 
    {
        public VictimofNight() : base("BB")
        {
            Name = "Victim of Night";
            Base.Text = @"Destroy target non-Vampire, non-Werewolf, non-Zombie creature.";
            throw new NotImplementedException();
        }
    }
}
