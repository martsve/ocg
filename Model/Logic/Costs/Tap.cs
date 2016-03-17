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

        public override bool TryToPay(Context Context, Player player, Card source)
        {
            if (CanPay(Context, player, source))
            {
                Context.Methods.Tap(source);
                return true;
            }
            return false;
        }

        public override bool CanPay(Context Context, Player player, Card source)
        {
            return !source.IsTapped;
        }
    }
}