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
            Card angelToken = new AngelToken();
            angelToken.Initializse(e.Game);
            angelToken.Owner = e.triggerPlayer;
            angelToken.Controller = e.triggerPlayer;

            // Select which object it attacks
            var legalAttackedObjects = e.Game.Logic.defender.Battlefield.Where(x => x.isType(CardType.Planeswalker))
                .Select(x => (GameObject)x)
                .Union(new List<GameObject> { e.Game.Logic.defender })
                .ToList();
            GameObject attacked_object = null;
            while (attacked_object == null) {
                if (legalAttackedObjects.Count() > 1)
                {
                    attacked_object = Owner.request.RequestFromObjects(RequestType.Attacking,
                        $"Select object for {angelToken} to attack", legalAttackedObjects);
                }
                else
                    attacked_object = legalAttackedObjects.First();
            }

            e.Game.Methods.CollectEvents();

            e.Game.Methods.ChangeZone(angelToken, Zone.None, Zone.Battlefield);
            angelToken.IsTapped = true;
            angelToken.IsAttacking = attacked_object;
            angelToken.IsBlocked = false;
            e.Game.Logic.attackers.Add(angelToken);

            angelTokenRef = angelToken.Referance;
            e.Game.Methods.AddDelayedTrigger(this, new Events.EndOfCombatStep( x => {
                x.Game.Methods.Exile(angelTokenRef);
            })
            {
                Text = $"At the beginning of the next combat step, exile {angelToken}."
            });

            e.Game.Methods.ReleaseEvents();
        }

        GameObjectReferance angelTokenRef;


        public void EndOfCombatExile(BaseEventInfo e)
        {
            e.Game.Methods.Exile(angelTokenRef);
        }
    }


    [Serializable]
    internal class AngelToken : Creature
    {
        public AngelToken() : base("", 4, 4)
        {
            Name = "Angel token";
            Subtype.Add("Angel");
            Supertype.Add("Token");

            AddKeyword(Keywords.Flying);
        }
    }
}
