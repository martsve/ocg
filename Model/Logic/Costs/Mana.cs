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

        public override bool TryToPay(Game game, Player player, Card source)
        {
            if (CanPay(game, player, source))
            {
                // TODO paying mana as cost??
                throw new NotImplementedException();
                return true;
            }
            return false;
        }

        public override bool CanPay(Game game, Player player, Card source)
        {
            return true;
        }
    }
}