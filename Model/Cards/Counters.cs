using System;
using Delver.LayerEffects;

namespace Delver
{
    [Serializable]
    internal abstract class Counter : GameObject
    {
        private GameObjectReferance _card;
        protected Game game;

        public Card Card
        {
            get { return _card.Card; }
            set { _card = value.Referance; }
        }

        public void Add(Game game, Card card)
        {
            this.game = game;
            Card = card;
            Add();
        }

        public virtual void Add()
        {
        }

        public virtual void Remove()
        {
        }
    }

    [Serializable]
    internal class PlussCounter : StatsCounter
    {
        public PlussCounter() : base(1, 1)
        {
        }
    }

    [Serializable]
    internal abstract class StatsCounter : Counter
    {
        public int Power;
        public int Thougness;

        private LayeredEffect _effect;

        protected StatsCounter(int power, int thoughness)
        {
            Power = power;
            Thougness = thoughness;
        }

        public override void Add()
        {
            _effect = new CounterLayer(Card, Power, Thougness);
            game.LayeredEffects.Add(_effect);
        }

        public override void Remove()
        {
            game.LayeredEffects.Remove(_effect);
        }
    }
}