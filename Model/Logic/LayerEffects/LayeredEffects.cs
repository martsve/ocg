using System;
using System.Linq;

namespace Delver.LayerEffects
{


    [Serializable]
    class CallBackLayer : LayeredEffect
    {
        private Action<BaseEventInfo> _callback;
        public CallBackLayer(Action<BaseEventInfo> callback, Duration duration, LayerType layer) : base(duration, layer)
        {
            this._callback = callback;
        }

        public override void Apply(BaseEventInfo e)
        {
            _callback.Invoke(e);
        }

        public override void End(BaseEventInfo e)
        {
            // TODO ???
        }
    }


    [Serializable]
    class CounterLayer : LayeredEffect
    {
        private readonly int _power;
        private readonly int _thoughness;
        private readonly Card _card;

        public CounterLayer(Card card, int power, int thoughness)
            : base(Duration.Continous, LayerType.PowerChanging_D_Counters)
        {
            _card = card;
            _power = power;
            _thoughness = thoughness;
        }

        public override void Apply(BaseEventInfo e)
        {
            _card.Current.Power += _power;
            _card.Current.Thoughness += _thoughness;
        }
    }


    [Serializable]
    class AlterPlayersCreaturesStats : LayeredEffect
    {
        private readonly int _power;
        private readonly int _thoughness;
        private readonly Player _player;

        public AlterPlayersCreaturesStats(Player player, int power, int thoughness, Duration duration)
            : base(duration, LayerType.PowerChanging_C_Modify)
        {
            _player = player;
            _power = power;
            _thoughness = thoughness;

            AffectedCards = _player.Battlefield.Where(c => c.isType(CardType.Creature)).ToList();
        }

        public override void Apply(BaseEventInfo e)
        {
            foreach (var card in AffectedCards)
            {
                card.Current.Power += _power;
                card.Current.Thoughness += _thoughness;
            }
        }

    }

}