//Ghoulraiser
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class Ghoulraiser : Creature 
    {
        public Ghoulraiser() : base("1BB", 2, 2)
        {
            Name = "Ghoulraiser";
            Base.Subtype.Add("Zombie");
            Base.Text = @"";
            
            Base.When(
                $"When Ghoulraiser enters the battlefield, return a Zombie card at random from your graveyard to your hand.",
                EventCollection.ThisEnterTheBattlefield(),
                returnRandomEffect
            );
        }

        public void returnRandomEffect(EventInfo e)
        {
            var zombies = e.SourcePlayer.Graveyard.Where(x => x.IsSubType("Zombie")).ToList();
            var N = e.Context.Rand.Next(zombies.Count);
            e.Context.Methods.ChangeZone(zombies[N], Zone.Graveyard, Zone.Battlefield);
        }
    }
}
