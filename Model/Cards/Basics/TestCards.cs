using System;
using Delver.AbilitiesSpace;

namespace Delver.Cards
{

    [Serializable]
    internal class Plains : Land
    {
        public Plains() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.White));
        }
    }

    [Serializable]
    internal class Island : Land
    {
        public Island() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Blue));
        }
    }

    [Serializable]
    internal class Swamp : Land
    {
        public Swamp() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Black));
        }
    }

    [Serializable]
    internal class Mountain : Land
    {
        public Mountain() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Red));
        }
    }


    [Serializable]
    internal class Forest : Land
    {
        public Forest() : base(CardType.Basic)
        {
            Base.CardAbilities.Add(new BasicLandAbility(Identity.Green));
        }
    }

}