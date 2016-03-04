using System;
using System.Collections.Generic;
using System.Linq;
using Delver.Interface;

namespace Delver
{
    [Flags]
    [Serializable]
    internal enum TargetType
    {
        Player = 1,
        Card = 2,
        Mana = 4
    }

    [Serializable]
    internal enum TargetValidation
    {
        NotSet = 0,
        Valid = 1,
        Invalid = -1,
    }


    internal interface ITarget
    {
        GameObject target { get; }
        bool Populate(Game game, Player player, Card source);
        TargetValidation ValidationStatus(Game game, Player player, Card source);
        TargetValidation Validate(Game game, Player player, Card source, GameObject target);
    }

    [Serializable]
    internal abstract class AbstractTarget : ITarget
    {
        public Func<GameObject, bool> Filter;
        public ITargetPopulator Populator;
        public List<ITargetValidator> Validators = new List<ITargetValidator>();

        public AbstractTarget(Func<GameObject, bool> filter = null)
        {
            this.Filter = filter;
        }

        public GameObjectReferance _target { get; set; }

        public GameObject target => _target.Object;

        public bool Populate(Game game, Player player, Card source)
        {
            var obj = Populator.Populate(game, player, source);
            if (obj == null)
                return false;
            _target = obj.Referance;
            return true;
        }

        public TargetValidation ValidationStatus(Game game, Player player, Card source)
        {
            if (target == null)
                return TargetValidation.Invalid;

            return Validate(game, player, source, target);
        }

        public TargetValidation Validate(Game game, Player player, Card source, GameObject target)
        {
            foreach (var validator in Validators)
            {
                if (validator.Validate(game, player, source, target) == TargetValidation.Valid)
                    return TargetValidation.Valid;
            }
            return TargetValidation.Invalid;
        }
    }

    internal interface ITargetValidator
    {
        TargetValidation Validate(Game game, Player player, Card source, GameObject target);
    }

    internal interface ITargetPopulator
    {
        GameObject Populate(Game game, Player player, Card source);
    }


    [Serializable]

    #region Target Validators

    internal class TargetValidator
    {
        [Serializable]
        public class PermanentOpponentControls : ITargetValidator
        {
            private readonly CardType type;

            public PermanentOpponentControls(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Game game, Delver.Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isType(type) && card.Zone == Zone.Battlefield && card.Controller != player)
                        return TargetValidation.Valid;
                    return TargetValidation.Invalid;
                }
                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class Permanent : ITargetValidator
        {
            private readonly CardType type;

            public Permanent(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Game game, Delver.Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isType(type) && card.Zone == Zone.Battlefield)
                        return TargetValidation.Valid;
                    return TargetValidation.Invalid;
                }
                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class Creature : Permanent
        {
            public Creature() : base(CardType.Creature)
            {
            }
        }

        [Serializable]
        public class Player : ITargetValidator
        {
            public TargetValidation Validate(Game game, Delver.Player player, Card source, GameObject target)
            {
                if (target is Delver.Player && ((Delver.Player) target).IsPlaying)
                    return TargetValidation.Valid;
                return TargetValidation.Invalid;
            }
        }
    }

    #endregion

    [Serializable]

    #region Target Populator

    internal class TargetPopulator
    {
        [Serializable]
        public class PermanentOpponentControls : ITargetPopulator
        {
            private readonly CardType type;

            public PermanentOpponentControls(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public GameObject Populate(Game game, Delver.Player player, Card source)
            {
                var list =
                    game.Methods.GetAllTargets(TargetType.Card)
                        .Where(o => ((Card) o).isType(type) && ((Card) o).Controller != player);
                return player.request.RequestFromObjects(RequestType.SelectTarget,
                    $"{player}, Select target for {source}", list);
            }
        }

        [Serializable]
        public class Permanent : ITargetPopulator
        {
            private readonly CardType type;

            public Permanent(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public GameObject Populate(Game game, Delver.Player player, Card source)
            {
                var list = game.Methods.GetAllTargets(TargetType.Card).Where(o => ((Card) o).isType(type));
                return player.request.RequestFromObjects(RequestType.SelectTarget,
                    $"{player}, Select target for {source}", list);
            }
        }

        [Serializable]
        public class Creature : Permanent
        {
            public Creature() : base(CardType.Creature)
            {
            }
        }

        [Serializable]
        public class Player : ITargetPopulator
        {
            public GameObject Populate(Game game, Delver.Player player, Card source)
            {
                var list = game.Methods.GetAllTargets(TargetType.Player);
                return player.request.RequestFromObjects(RequestType.SelectTarget,
                    $"{player}, Select target for {source}", list);
            }
        }

        [Serializable]
        public class CreatureOrPlayer : ITargetPopulator
        {
            public GameObject Populate(Game game, Delver.Player player, Card source)
            {
                var list = game.Methods.GetAllTargets(TargetType.Card | TargetType.Player).Where(o =>
                {
                    if (o is Card)
                        return ((Card) o).isType(CardType.Creature);
                    return true;
                });
                return player.request.RequestFromObjects(RequestType.SelectTarget,
                    $"{player}, Select target for {source}", list);
            }
        }
    }

    #endregion

    [Serializable]

    #region Target Definitions

    internal class Target
    {
        [Serializable]
        public class Permanent : AbstractTarget
        {
            public Permanent(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.Permanent(type));
                Populator = new TargetPopulator.Permanent(type);
            }
        }

        [Serializable]
        public class PermanentOpponentControls : AbstractTarget
        {
            public PermanentOpponentControls(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.PermanentOpponentControls(type));
                Populator = new TargetPopulator.PermanentOpponentControls(type);
            }
        }

        [Serializable]
        public class Creature : AbstractTarget
        {
            public Creature(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.Permanent(CardType.Creature));
                Populator = new TargetPopulator.Permanent(CardType.Creature);
            }
        }

        [Serializable]
        public class Player : AbstractTarget
        {
            public Player(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.Player());
                Populator = new TargetPopulator.Player();
            }
        }

        [Serializable]
        public class CreatureOrPlayer : AbstractTarget
        {
            public CreatureOrPlayer(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.Creature());
                Validators.Add(new TargetValidator.Player());
                Populator = new TargetPopulator.CreatureOrPlayer();
            }
        }
    }

    #endregion
}