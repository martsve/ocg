using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{

    [Serializable]
    internal abstract class Card : GameObject
    {
        protected CardBase Base { get; set; } = new CardBase();
        public CardBase Current { get; set; } = new CardBase();

        protected Card()
        {
            Base.Name = GetType().Name;
        }

        public void ApplyBase()
        {
            Current.ApplyBase(Base);
        }

        protected Card(CardType cardType) : this()
        {
            Base.SetType(cardType);
        }

        public bool isType(CardType ask)
        {
            var result = (Current.type & ask) == ask;
            return result;
        }

        // Game modified parameters
        public bool IsTapped { get; set; }
        public bool SummonSickness => UntapController != Controller;
        public GameObject IsAttacking { get; set; }
        public List<Card> IsBlocking { get; set; }
        public List<Card> DamageAssignmentOrder { get; set; }
        public bool IsBlocked { get; set; }
        public Player UntapController { get; set; }
        public int Damage { get; set; }
        public List<Counter> Counters { get; set; } = new List<Counter>();
        public ManaCost PlayedWith { get; set;  } = new ManaCost();
        public Zone Zone = Zone.Library;
        public Player Owner { get; set; }
        public Player Controller { get; set; }

        public bool HasActivatedAbilities()
        {
            foreach (var a in Current.CardAbilities)
                if (a.type == AbiltiyType.Activated)
                    return true;
            return false;
        }

        public bool Has(Keywords keyword)
        {
            return Current.keywords.Contains(keyword);
        }

        public bool HasManaSource(Game game, Player player, Card card)
        {
            foreach (var ability in Current.CardAbilities)
                if (ability.IsManaSource && ability.CanPay(game, player, card))
                    return true;
            return false;
        }

        public bool CanBeTargeted(Player player, Card source)
        {
            if (this.Has(Keywords.Shroud))
                return false;

            if (this.Has(Keywords.Hexproof) && this.Controller != player)
                return false;

            return true;
        }


        public bool IsColor(Identity color)
        {
            return (Current.Color & color) == color;
        }

        public void SetZone(Game game, Zone from, Zone to)
        {
            ApplyBase();
            UntapController = null;
            Zone = to;
            SetNewZoneId();
            IsAttacking = null;
            IsTapped = false;
            Timestamp = game.GetTimestamp();
            game.Methods.RemoveCounters(this);
        }

        public void SetOwner(Player p)
        {
            Owner = p;
            Controller = p;
        }

        public override string ToString()
        {
            if (Owner != null)
                return $"{Current.Name}_{Owner}_{Id}";
            return $"{Current.Name}_{Id}";
        }

        public bool IsCastable(Game game)
        {
            if (Has(Keywords.Flash) || Current.type == CardType.Instant)
                return true;

            if (game.ActivePlayer != Controller)
                return false;

            if (game.CurrentStep.type != StepType.PostMain && game.CurrentStep.type != StepType.PreMain)
                return false;

            if (game.CurrentStep.stack.Count > 0)
                return false;

            return true;
        }

    }
}