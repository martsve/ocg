using System;

namespace Delver
{
    [Serializable]
    internal class GainLifeEffect : Effect
    {
        private readonly int life;

        public GainLifeEffect(int life)
        {
            this.life = life;
            Text = $"You gain {life} life";
        }

        public override void Invoke(EventInfo e)
        {
            e.Context.Methods.GainLife(e.SourcePlayer, e.SourceCard, life);
        }
    }

    [Serializable]
    internal class LookTopOfLibraryPickCardsRestGraveEffect : Effect
    {
        private readonly int lookAt;
        private readonly int pickCards;

        public LookTopOfLibraryPickCardsRestGraveEffect(int lookAt, int pickCards)
        {
            this.lookAt = lookAt;
            this.pickCards = pickCards;
            Text = $"Look at the top {lookAt} cards of your library. Put {pickCards} into your hand and the rest into your graveyard.";
        }

        public override void Invoke(EventInfo e)
        {
            throw new NotImplementedException();
        }
    }
}