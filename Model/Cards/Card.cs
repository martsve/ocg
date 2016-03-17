using System;
using System.Collections.Generic;
using System.Linq;
using Delver.LayerEffects;

namespace Delver
{

    [Serializable]
    internal abstract class Card : GameObject
    {
        public string Name { get; set; } = null;

        protected Card()
        {
            Name = GetType().Name;
        }

        protected Card(CardType cardType) : this()
        {
            Base.SetCardType(cardType);
        }

        protected CardBase Base { get; set; } = new CardBase();
        public CardBase Current { get; set; } = new CardBase();

        public void Initialize(Game game, Player player)
        {
            Initialize(game);

            SetOwner(player);

            ApplyBase();

            game.Methods.AbsorbEvents(this);
        }

        public void ApplyBase()
        {
            Current.ApplyBase(Base);
        }

        // Game modified parameters
        public bool IsTapped { get; set; }
        public GameObject IsAttacking { get; set; }
        public List<Card> IsBlocking { get; set; } = new List<Card>();
        public List<Card> DamageAssignmentOrder { get; set; } = new List<Card>();
        public bool IsBlocked { get; set; }
        public Player UntapController { get; set; }
        public int Damage { get; set; }
        public bool DeathtouchDamage { get; set; }
        public List<Counter> Counters { get; set; } = new List<Counter>();
        public ManaCost PlayedWith { get; set;  } = new ManaCost();
        public Zone Zone = Zone.Library;
        public Player Owner { get; set; }
        public Player Controller { get; set; }

        // Reset all game modified paramaeters
        public void SetZone(Game game, Zone from, Zone to)
        {
            ApplyBase();

            IsTapped = false;
            UntapController = null;
            IsAttacking = null;
            IsBlocking.Clear();
            DamageAssignmentOrder.Clear();
            IsBlocked = false;
            Damage = 0;
            DeathtouchDamage = false;
            game.Methods.RemoveCounters(this);
            PlayedWith = null;

            Zone = to;
            SetNewZoneId();

            Timestamp = game.GetTimestamp();
        }

        public bool SummonSickness => UntapController != Controller;

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

        public bool isCardType(CardType ask)
        {
            var result = (Current.CardType & ask) == ask;
            return result;
        }

        public bool IsCastable(Game game)
        {
            if (Has(Keywords.Flash) || Current.CardType == CardType.Instant)
                return true;

            if (game.ActivePlayer != Controller)
                return false;

            if (game.CurrentStep.type != StepType.PostMain && game.CurrentStep.type != StepType.PreMain)
                return false;

            if (game.CurrentStep.stack.Count > 0)
                return false;

            return true;
        }

        public void SetOwner(Player p)
        {
            Owner = p;
            Controller = p;
        }

        public override string ToString()
        {
            if (Owner != null)
                return $"{Name}_{Owner}_{Id}";
            return $"{Name}_{Id}";
        }

        /// <summary>
        /// When an aura resolves it calls Enchant (AuraEffect)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="enchantedObject"></param>
        public void Enchant(EventInfo e, GameObject enchantedObject)
        {
            Base.EnchantedObject = enchantedObject.Referance;
            ApplyBase();
            foreach (var layer in Current.FollowingLayers)
            {
                layer.Following = this.Referance;
                e.Game.LayeredEffects.Add(layer);
            }
        }

    }
}