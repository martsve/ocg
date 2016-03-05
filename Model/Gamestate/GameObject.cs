using System;
using System.Collections.Generic;

namespace Delver
{
    [Serializable]
    internal class GameObject
    {
        public GameObject()
        {
            Marks = new Dictionary<string, object>();
        }

        public int Id { get; private set; }
        public int ZoneId { get; set; }

        private Rand Random { get; set; }

        public GameObjectReferance Referance => new GameObjectReferance(this);

        public Dictionary<string, object> Marks { get; set; }

        public int Timestamp { get; set; }

        public void Initializse(Game game)
        {
            Random = game.Rand;
            SetId();
            SetNewZoneId();
        }

        public void SetNewZoneId()
        {
            ZoneId = Random.Unique();
        }

        private void SetId()
        {
            Id = Random.Unique();
        }

    }


    [Serializable]
    internal class GameObjectReferance
    {
        private readonly GameObject _gameObject;
        private readonly int _zoneId;

        public GameObjectReferance(GameObject gameObject)
        {
            _gameObject = gameObject;
            _zoneId = gameObject.ZoneId;
        }

        public GameObject Object => _gameObject.ZoneId == _zoneId ? _gameObject : null;

        public Card Card => _gameObject.ZoneId == _zoneId ? (Card) _gameObject : null;

        public Player Player => _gameObject.ZoneId == _zoneId ? (Player) _gameObject : null;
    }
}