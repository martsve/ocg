using System;

namespace Delver.Costs
{
    [Serializable]
    internal class OnlyAsSorceryCost : Cost
    {
        public OnlyAsSorceryCost()
        {
        }

        public override bool TryToPay(Game game, Player player, Card source)
        {
            return CanPay(game, player, source);
        }

        public override bool CanPay(Game game, Player player, Card source)
        {
            return player.CanPlaySorcery(game);
        }
    }
}