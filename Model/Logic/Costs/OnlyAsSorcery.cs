using System;

namespace Delver.Costs
{
    [Serializable]
    internal class OnlyAsSorceryCost : Cost
    {
        public OnlyAsSorceryCost()
        {
        }

        public override bool TryToPay(Context context, Player player, Card source)
        {
            return CanPay(context, player, source);
        }

        public override bool CanPay(Context context, Player player, Card source)
        {
            return player.CanPlaySorcery(context);
        }
    }
}