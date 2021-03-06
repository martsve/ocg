//MistRaven
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MistRaven : Creature 
    {
        public MistRaven() : base("2UU", 2, 2)
        {
            Name = "Mist Raven";
            Base.Subtype.Add("Bird");
            Base.AddKeyword(Keywords.Flying);

            Base.When(
                 $"When Mist Raven enters the battlefield, return target creature to its owner's hand.",
                 EventCollection.ThisEnterTheBattlefield(),
                 new ReturnCreatureEffect(),
                 new Target.Creature()
             );

        }
    }
}
