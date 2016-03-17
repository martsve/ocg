//UnbreathingHorde
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class UnbreathingHorde : Creature 
    {
        public UnbreathingHorde() : base("Creature � Zombie 0/0, 2B (3)")
        {
            Name = "Unbreathing Horde";
            Current.Text = @"Unbreathing Horde enters the battlefield with a +1/+1 counter on it for each other Zombie you control and each Zombie card in your graveyard. If Unbreathing Horde would be dealt damage, prevent that damage and remove a +1/+1 counter from it.";
            throw new NotImplementedException();
        }
    }
}
