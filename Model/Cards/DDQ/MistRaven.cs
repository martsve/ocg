//MistRaven
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class MistRaven : Creature 
    {
        public MistRaven() : base("Creature � Bird 2/2, 2UU (4)")
        {
            Name = "Mist Raven";
            Current.Text = @"Flying When Mist Raven enters the battlefield, return target creature to its owner's hand.";
            throw new NotImplementedException();
        }
    }
}
