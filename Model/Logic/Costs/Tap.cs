using System;

namespace Delver.Costs
{
    [Serializable]
    internal class TapCost : Cost
    {
        public TapCost()
        {
            Text = "{T}";
        }

        public override bool TryToPay(Game game, Player player, Card source)
        {
            if (CanPay(game, player, source))
            {
                game.Methods.Tap(source);
                return true;
            }
            return false;
        }

        public override bool CanPay(Game game, Player player, Card source)
        {
            return !source.IsTapped;
        }
    }
}