//GoldnightRedeemer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class GoldnightRedeemer : Creature 
    {
        public GoldnightRedeemer() : base("4WW", 4, 4)
        {
            Name = "Goldnight Redeemer";
            Base.Subtype.Add("Angel");
            Base.AddKeyword(Keywords.Flying);
            
            Base.When(
                $"When Goldnight Redeemer enters the battlefield, you gain 2 life for each other creature you control.",
                EventCollection.ThisEnterTheBattlefield(),
                gainLife
            );
        }

        public void gainLife(EventInfo e)
        {
            int otherCreatures = e.SourcePlayer.Battlefield.Where(x => x != this).Count();
            e.Context.Methods.GainLife(e.SourcePlayer, this, otherCreatures * 2);
        }
    }
}
