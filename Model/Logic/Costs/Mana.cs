using System;

namespace Delver.Costs
{
    [Serializable]
    internal class PayManaCost : Cost
    {
        ManaCost cost;
        public PayManaCost(string mana)
        {
            cost = new ManaCost(mana);
            Text = cost.ToString();
        }

        public override bool TryToPay(Context context, Player player, Card source)
        {
            if (CanPay(context, player, source))
                return context.Logic.TryToPay(player, cost, source);
            return false;
        }

        public override bool CanPay(Context context, Player player, Card source)
        {
            return true;
        }
    }
}