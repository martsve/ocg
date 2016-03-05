using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;
using Delver.Effects;

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

            Events.Add(new Events.ThisAttacks(new CallbackEffect(ThisAttacks))
            {
                Text = $"Whenever {Name} attacks, put a 4/4 white Angel creature token with flying onto the battlefield tapped and attacking. Exile that token at end of combat."
            });
        }

        public void ThisAttacks(BaseEventInfo e)
        {
            Card angelToken = new AngelToken(4, 4);
            e.Game.Methods.AddTokenAttacking(e.triggerPlayer, angelToken).IsTapped = true;
            angelTokenRef = angelToken.Referance;
            e.Game.Methods.AddDelayedTrigger(
                this, 
                new Events.EndOfCombatStep(new CallbackEffect( x => { x.Game.Methods.Exile(angelTokenRef); }))
                    { Text = $"Exile that token at end of combat."}
            );

        }

    }

}
