using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class GeistOfSaintTraft : Creature
    {
        GameObjectReferance angelTokenRef;

        public GeistOfSaintTraft() : base("1WU", 2, 2)
        {
            Name = "Geist of Saint Traft";
            Subtype.Add("Spirit");
            Subtype.Add("Cleric");
            Supertype.Add("Legendary");

            AddKeyword(Keywords.Hexproof);

            Events.Add(new Events.ThisAttacks(ThisAttacks)
            {
                Text = $"Whenever {Name} attacks, put a 4/4 white Angel creature token with flying onto the battlefield tapped and attacking. Exile that token at end of combat."
            });
        }

        public void ThisAttacks(BaseEventInfo e)
        {
            Card angelToken = new AngelToken(e.Game, e.triggerPlayer, 4, 4);
            e.Game.Methods.AddTokenAttacking(angelToken).IsTapped = true;
            angelTokenRef = angelToken.Referance;
            e.Game.Methods.AddDelayedTrigger(
                this, 
                new Events.EndOfCombatStep( x => { x.Game.Methods.Exile(angelTokenRef); })
                    { Text = $"At the beginning of the next combat step, exile {angelToken}."}
            );

        }

    }


    [Serializable]
    internal class AngelToken : CreatureToken
    {
        public AngelToken(Game game, Player player, int power, int thoughness) : base(game, player, power, thoughness)
        {
            Name = "Angel token";
            Subtype.Add("Angel");
            Supertype.Add("Token");
            AddKeyword(Keywords.Flying);
        }
    }

}
