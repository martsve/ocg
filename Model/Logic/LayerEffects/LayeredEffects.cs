using System;
using System.Linq;

namespace Delver.LayerEffects
{

    [Serializable]
    class CounterEffect : LayeredEffect
    {
        private readonly int _power;
        private readonly int _thoughness;
        private readonly Card _card;

        public CounterEffect(Card card, int power, int thoughness)
            : base(Duration.Continous, Layers.PowerChanging_D_Counters)
        {
            _card = card;
            _power = power;
            _thoughness = thoughness;
        }

        public override void Invoke()
        {
            _card.Power += _power;
            _card.Thoughness += _thoughness;
        }
    }


    [Serializable]
    class AlterPlayersCreaturesStats : LayeredEffect
    {
        private readonly int _power;
        private readonly int _thoughness;
        private readonly Player _player;

        public AlterPlayersCreaturesStats(Player player, int power, int thoughness, Duration duration)
            : base(duration, Layers.PowerChanging_C_Modify)
        {
            _player = player;
            _power = power;
            _thoughness = thoughness;
        }

        public override void Invoke()
        {
            AffectedCards = _player.Battlefield.Where(c => c.isType(CardType.Creature)).ToList();
            foreach (var card in AffectedCards)
            {
                card.Power += _power;
                card.Thoughness += _thoughness;
            }
        }
    }

}