//TandemLookout
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class TandemLookout : Creature 
    {
        public TandemLookout() : base("2U", 2, 1)
        {
            Name = "Tandem Lookout";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Scout");
            Base.AddKeyword(Keywords.Soulbond);
            Base.Text = @"As long as Tandem Lookout is paired with another creature, each of those creatures has 'Whenever this creature deals damage to an opponent, draw a card.'";
            NotImplemented();


        }
    }
}
