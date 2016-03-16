using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.LayerEffects;

namespace Delver
{
    [Serializable]
    class CardBase
    {
        public void ApplyBase(CardBase cardBase)
        {
            this.Power = cardBase.Power;
            this.Thoughness = cardBase.Thoughness;
            this.CardAbilities = cardBase.CardAbilities;
            this.FollowingLayers = cardBase.FollowingLayers;
            this.keywords = cardBase.keywords;
            this.Text = cardBase.Text;
            this.CanAttack = cardBase.CanAttack;
            this.CanBlock = cardBase.CanBlock;
            this.CastingCost = cardBase.CastingCost;
            this.Events = cardBase.Events;
            this.Color = cardBase.Color;
            this.Subtype = cardBase.Subtype;
            this.Supertype = cardBase.Supertype;
            this.type = cardBase.type;
            this.Name = cardBase.Name;
        }

        public string Name { get; set; } = null;

        public int Power { get; set; } = 0;

        public int Thoughness { get; set; } = 0;

        public Abilities CardAbilities { get; set; } = new Abilities();

        public List<LayeredEffect> FollowingLayers { get; set; } = new List<LayeredEffect>();

        public List<Keywords> keywords { get; set; } = new List<Keywords>();

        public string Text { get; set; } = null;

        public bool CanAttack { get; set; } = true;

        public bool CanBlock { get; set; } = true;

        public ManaCost CastingCost { get; set; } = new ManaCost();

        public List<CustomEventHandler> Events { get; set; } = new List<CustomEventHandler>();

        public Identity Color { get; set; } = Identity.Colorless;

        public List<string> Subtype { get; set; } = new List<string>();

        public List<string> Supertype { get; set; } = new List<string>();

        public CardType type { get; set; }

        public GameObjectReferance EnchantedObject { get; set; }

        public void SetType(CardType cardType)
        {
            type = cardType;
        }

        public void AddType(CardType cardType)
        {
            type = type | cardType;
        }

        public void SetColor(Identity color)
        {
            this.Color = color;
        }

        public void AddKeyword(Keywords keyword)
        {
            keywords.Add(keyword);
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

        public void When(string text, CustomEventHandler handler, Effect effect, params ITarget[] targets)
        {
            effect.AddTarget(targets);
            handler.effect = effect;
            handler.Text = text;
            Events.Add(handler);
        }

        public void When(string text, CustomEventHandler handler, Action<BaseEventInfo> callback, params ITarget[] targets)
        {
            var effect = new CallbackEffect(callback);
            effect.AddTarget(targets);
            handler.effect = effect;
            handler.Text = text;
            Events.Add(handler);
        }

        /// <summary>
        /// Methods to add following layers
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="layer"></param>
        /// <param name="layerType"></param>
        public void Following(Action<BaseEventInfo> callback, LayerType layerType)
        {
            var layer = new CallBackLayer(callback, Duration.Following, layerType);
            FollowingLayers.Add(layer);
        }

    }
}
