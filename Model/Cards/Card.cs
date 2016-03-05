using System;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{
    [Serializable]
    internal abstract partial class Card : GameObject
    {
        public CardType type { get; private set; }

        public bool isType(CardType ask)
        {
            var result = (type & ask) == ask;
            return result;
        }

        public void SetType(CardType cardType)
        {
            type = cardType;
        }

        public void AddType(CardType cardType)
        {
            type = type | cardType;
        }
    }

    internal abstract partial class Card : GameObject
    {

        public Abilities Abilities = new Abilities();

        private readonly List<Keywords> keywords = new List<Keywords>();
        public bool IsTapped { get; set; }

        public bool SummonSickness => UntapController != Controller;

        public GameObject IsAttacking { get; set; }
        public List<Card> IsBlocking { get; set; }
        public List<Card> DamageAssignmentOrder { get; set; }
        public bool IsBlocked { get; set; }

        public Player UntapController { get; set; }

        public int Power { get; set; }
        public int Thoughness { get; set; }

        public int BasePower { get; set; }
        public int BaseThoughness { get; set; }

        public int Damage { get; set; }


        public List<Counter> Counters { get; }

        public bool HasActivatedAbilities()
        {
            foreach (var a in Abilities)
                if (a.type == AbiltiyType.Activated)
                    return true;
            return false;
        }

        public void AddKeyword(Keywords keyword)
        {
            keywords.Add(keyword);
        }

        public bool Has(Keywords keyword)
        {
            return keywords.Contains(keyword);
        }


        public bool HasManaSource(Game game, Player player, Card card)
        {
            foreach (var ability in Abilities)
                if (ability.IsManaSource && ability.CanPay(game, player, card))
                    return true;
            return false;
        }

    }


    internal abstract partial class Card : GameObject
    {
        public ManaCost CastingCost = new ManaCost();

        public List<CustomEventHandler> Events = new List<CustomEventHandler>();

        public ManaCost PlayedWith = new ManaCost();
        public List<string> Subtype = new List<string>();

        public List<string> Supertype = new List<string>();

        public Zone Zone = Zone.Library;

        protected Card()
        {
            Name = GetType().Name;
            Color = Identity.Colorless;
            Marks = new Dictionary<string, object>();
            Counters = new List<Counter>();
        }

        protected Card(CardType cardType) : this()
        {
            SetType(cardType);
        }

        public Player Owner { get; set; }
        public Player Controller { get; set; }

        public string Name { get; set; }

        public Identity Color { get; set; }

        public void SetColor(Identity color)
        {
            this.Color = color;
        }

        public bool IsColor(Identity color)
        {
            return (this.Color & color) == color;
        }

        public void SetZone(Game game, Zone from, Zone to)
        {
            UntapController = null;
            Zone = to;
            SetNewZoneId();
            Power = BasePower;
            Thoughness = BaseThoughness;
            IsAttacking = null;
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
                return $"{Name}_{Owner}_{Id}";
            return $"{Name}_{Id}";
        }

        public bool IsCastable(Game game)
        {
            if (Has(Keywords.Flash) || type == CardType.Instant)
                return true;

            if (game.ActivePlayer != Controller)
                return false;

            if (game.CurrentStep.type != StepType.PostMain && game.CurrentStep.type != StepType.PreMain)
                return false;

            if (game.CurrentStep.stack.Count > 0)
                return false;

            return true;
        }

        public void SetCastingCost(string mana)
        {
            CastingCost = new ManaCost(mana);
            SetDefaultIdentity();
        }

        public void SetDefaultIdentity()
        {
            SetColor(CastingCost.getIdentity());
        }
    }
}