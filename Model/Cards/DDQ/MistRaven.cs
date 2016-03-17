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
        public MistRaven() : base("2UU", 2, 2)
        {
            Name = "Mist Raven";
            Base.Subtype.Add("Bird");
            Base.Text = @"Flying When Mist Raven enters the battlefield, return target creature to its owner's hand.";
            throw new NotImplementedException();
        }
    }
}
