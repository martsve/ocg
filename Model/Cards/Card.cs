using System;
using System.Collections.Generic;
using System.Linq;
using Delver.LayerEffects;

namespace Delver
{

    /// <summary>
    /// Card object. Represents all cards in the game.
    /// </summary>
    [Serializable]
    internal abstract class Card : GameObject
    {
        public string Name { get; set; } = null;
        protected CardBase Base { get; set; } = new CardBase();
        public CardBase Current { get; set; } = new CardBase();

        public bool IsTapped { get; set; }
        public GameObject IsAttacking { get; set; }
        public List<Card> IsBlocking { get; set; } = new List<Card>();
        public List<Card> DamageAssignmentOrder { get; set; } = new List<Card>();
        public bool IsBlocked { get; set; }
        public Player UntapController { get; set; }
        public int Damage { get; set; }
        public bool DeathtouchDamage { get; set; }
        public List<Counter> Counters { get; set; } = new List<Counter>();
        public ManaCost PlayedWith { get; set; } = new ManaCost();
        public Zone Zone = Zone.Library;
        public Player Owner { get; set; }
        public Player Controller { get; set; }

        /// <summary>
        /// Default constructor. Creates a card
        /// </summary>
        protected Card()
        {
            Name = GetType().Name;
        }

        /// <summary>
        /// Create a card of a certain type
        /// </summary>
        /// <param name="cardType"></param>
        protected Card(CardType cardType) : this()
        {
            Base.SetCardType(cardType);
        }

        /// <summary>
        /// Initialize a card for use in a game
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="player"></param>
        public void Initialize(Context Context, Player player)
        {
            Initialize(Context);
            SetOwner(player);
            ApplyBase();
            Context.Methods.AbsorbEvents(this);
        }

        /// <summary>
        /// Reassign all base-values of a card
        /// </summary>
        public void ApplyBase()
        {
            Current.ApplyBase(Base);
        }
        
        /// <summary>
        /// Perform all reset-effects that takes place when a card changes zone.
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetZone(Context Context, Zone from, Zone to)
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
            Context.Methods.RemoveCounters(this);
            PlayedWith = null;

            Zone = to;
            SetNewZoneId();

            Timestamp = Context.GetTimestamp();
        }

        /// <summary>
        /// Checks if the card has summon sickness
        /// </summary>
        public bool HasSummonSickness()
        {
          return UntapController != Controller;
        } 
        
        /// <summary>
        /// Checks if the card has a certain keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool Has(Keywords keyword)
        {
            return Current.Keywords.Contains(keyword);
        }

        /// <summary>
        /// Checks if the card has any activated abilities
        /// </summary>
        /// <returns></returns>
        public bool HasActivatedAbilities()
        {
            var zone = this.Zone;
            foreach (var a in Current.CardAbilities)
                if (a.type == AbiltiyType.Activated)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks if the card has any activated abilities
        /// </summary>
        /// <returns></returns>
        public bool CanActivateAbilities(Context context, Player player)
        {
            var zone = this.Zone;
            foreach (var a in Current.CardAbilities)
                if (a.type == AbiltiyType.Activated && a.ActiveZone.Contains(zone) && a.CanPay(context, player, this))
                    return true;
            return false;
        }

        /// <summary>
        /// Checks if the card has any mana source abilities
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool HasManaSource(Context Context, Player player, Card card)
        {
            foreach (var ability in Current.CardAbilities)
                if (ability.IsManaSource && ability.CanPay(Context, player, card))
                    return true;
            return false;
        }

        /// <summary>
        /// Checks if the card can be targeted by a source
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool CanBeTargeted(Player player, Card source)
        {
            if (this.Has(Keywords.Shroud))
                return false;

            if (this.Has(Keywords.Hexproof) && this.Controller != player)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the card is of a certain color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool IsColor(Identity color)
        {
            return (Current.Color & color) == color;
        }

        /// <summary>
        /// Checks if the card is of a certain type
        /// </summary>
        /// <param name="ask"></param>
        /// <returns></returns>
        public bool isCardType(CardType ask)
        {
            var result = (Current.CardType & ask) == ask;
            return result;
        }

        /// <summary>
        /// Checks if this card is castable
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public bool IsCastable(Context Context)
        {
            if (Has(Keywords.Flash) || Current.CardType == CardType.Instant)
                return true;

            if (this.Zone != Zone.Hand && !(Has(Keywords.Flashback) && this.Zone == Zone.Graveyard))
                return false;

            if (Context.ActivePlayer != Controller)
                return false;

            if (Context.CurrentStep.type != StepType.PostMain && Context.CurrentStep.type != StepType.PreMain)
                return false;

            if (Context.CurrentStep.stack.Count > 0)
                return false;

            return true;
        }

        /// <summary>
        /// Sets the owner of a card
        /// </summary>
        /// <param name="player"></param>
        public void SetOwner(Player player)
        {
            this.Owner = player;
            this.Controller = player;
        }

        /// <summary>
        /// Convert card to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Id}";
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
                e.Context.LayeredEffects.Add(layer);
            }
        }

        public void NotImplemented()
        {
            throw new NotImplementedException();
        }

    }
}