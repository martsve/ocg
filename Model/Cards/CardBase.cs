using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.LayerEffects;

namespace Delver
{
    [Serializable]
    internal class CardBase
    {
        public void ApplyBase(CardBase cardBase)
        {
            this.Power = cardBase.Power;
            this.Thoughness = cardBase.Thoughness;
            this.CardAbilities = cardBase.CardAbilities;
            this.FollowingLayers = cardBase.FollowingLayers;
            this.Keywords = cardBase.Keywords;
            this.Text = cardBase.Text;
            this.CanAttack = cardBase.CanAttack;
            this.CanBlock = cardBase.CanBlock;
            this.CastingCost = cardBase.CastingCost;
            this.Events = cardBase.Events;
            this.Color = cardBase.Color;
            this.Subtype = cardBase.Subtype;
            this.Supertype = cardBase.Supertype;
            this.CardType = cardBase.CardType;
            this.EnchantedObject = cardBase.EnchantedObject;
        }

        public int Power { get; set; } = 0;

        public int Thoughness { get; set; } = 0;

        public Abilities CardAbilities { get; set; } = new Abilities();

        public List<LayeredEffect> FollowingLayers { get; set; } = new List<LayeredEffect>();

        public List<Keywords> Keywords { get; set; } = new List<Keywords>();

        public string Text { get; set; } = null;

        public bool CanAttack { get; set; } = true;

        public bool CanBlock { get; set; } = true;

        public ManaCost CastingCost { get; set; } = new ManaCost();

        public List<EventListener> Events { get; set; } = new List<EventListener>();

        public Identity Color { get; set; } = Identity.Colorless;

        public List<string> Subtype { get; set; } = new List<string>();

        public List<string> Supertype { get; set; } = new List<string>();

        public CardType CardType { get; set; }

        public GameObjectReferance EnchantedObject { get; set; }


        public void SetCardType(CardType cardType)
        {
            CardType = cardType;
        }

        public void AddCardType(CardType cardType)
        {
            CardType = CardType | cardType;
        }

        public void SetColor(Identity color)
        {
            this.Color = color;
        }

        public void AddKeyword(Keywords keyword)
        {
            Keywords.Add(keyword);
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

        public void Effect(string text, Effect effect, params ITarget[] targets)
        {
            effect.Text = text;
            effect.AddTarget(targets);
            CardAbilities.Add(effect);
        }

        public void Effect(Effect effect, params ITarget[] targets)
        {
            effect.AddTarget(targets);
            CardAbilities.Add(effect);
        }

        /// <summary>
        /// Add a triggered event listender.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="listener"></param>
        /// <param name="effect"></param>
        /// <param name="targets"></param>
        public void When(string text, EventListener listener, Effect effect, params ITarget[] targets)
        {
            effect.AddTarget(targets);
            listener.Effect = effect;
            listener.Text = text;
            Events.Add(listener);
        }

        /// <summary>
        /// Add a triggered event listender.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        /// <param name="targets"></param>
        public void When(string text, EventListener listener, Action<EventInfo> callback, params ITarget[] targets)
        {
            var effect = new CallbackEffect(callback);
            effect.AddTarget(targets);
            listener.Effect = effect;
            listener.Effect.Text = text;
            listener.Text = text;
            Events.Add(listener);
        }

        /// <summary>
        /// Add a layer that follows an object.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="layer"></param>
        /// <param name="layerType"></param>
        public void Following(Action<EventInfo> callback, LayerType layerType)
        {
            var layer = new CallBackLayer(callback, Duration.Following, layerType);
            FollowingLayers.Add(layer);
        }

    }
}
