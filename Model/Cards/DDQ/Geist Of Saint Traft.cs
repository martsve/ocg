﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.Tokens;

//namespace Delver.Cards.DDQ
namespace Delver.Cards.TestCards
{
    [Serializable]
    internal class GeistOfSaintTraft : Creature
    {
        GameObjectReferance angelTokenRef;

        public GeistOfSaintTraft() : base("1WU", 2, 2)
        {
            Base.Name = "Geist of Saint Traft";
            Base.Subtype.Add("Spirit");
            Base.Subtype.Add("Cleric");
            Base.Supertype.Add("Legendary");

            Base.AddKeyword(Keywords.Hexproof);

            Base.When(
                $"Whenever {Base.Name} attacks, put a 4/4 white Angel creature token with flying onto the battlefield tapped and attacking. Exile that token at end of combat.",
                EventCollection.ThisAttacks(),
                ThisAttacks
            );
        }

        public void ThisAttacks(BaseEventInfo e)
        {
            Card angelToken = new AngelToken(4, 4);
            e.Game.Methods.AddTokenAttacking(e.triggerPlayer, angelToken).IsTapped = true;
            angelTokenRef = angelToken.Referance;

            e.AddDelayedTrigger(
                $"Exile that token at end of combat.",
                EventCollection.EndOfCombatStep(),
                x => { x.Game.Methods.Exile(angelTokenRef); }
             );

        }

    }

}
