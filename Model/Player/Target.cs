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
        bool Populate(Game game, Player player, Card source, List<GameObject> selected);
        TargetValidation ValidationStatus(Game game, Player player, Card source);
        TargetValidation Validate(Game game, Player player, Card source, GameObject target);
    }

    [Serializable]
    internal abstract class AbstractTarget : ITarget
    {
        public Func<GameObject, bool> Filter;
        public TargetPopulator Populator;
        public List<ITargetValidator> Validators = new List<ITargetValidator>();

        public AbstractTarget(Func<GameObject, bool> filter = null)
        {
            this.Filter = filter;
        }

        public AbstractTarget Clone => (AbstractTarget)this.MemberwiseClone();

        public GameObjectReferance _target { get; set; }

        public GameObject target => _target.Object;

        public bool Populate(Game game, Player player, Card source, List<GameObject> selected)
        {
            var obj = Populator.Populate(game, player, source, Validators, selected);
            if (obj == null)
                return false;
            _target = obj.Referance;
            return true;
        }

        public TargetValidation ValidationStatus(Game game, Player player, Card source)
        {
            if (target == null)
                return TargetValidation.Invalid;

            return Validators.Validate(game, player, source, target);
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

    static class TargetValidatorExtension
    {
        public static TargetValidation Validate(this List<ITargetValidator> validators, Game game, Player player, Card source, GameObject target)
        {
            foreach (var validator in validators)
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


    [Serializable]

    #region Target Validators

    internal class TargetValidator
    {
        public class ValidatePermanentYouControl : ITargetValidator
        {
            private readonly CardType type;

            public ValidatePermanentYouControl(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Game game, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isType(type) && card.Zone == Zone.Battlefield && card.Controller == player)
                        return TargetValidation.Valid;
                    return TargetValidation.Invalid;
                }
                return TargetValidation.Invalid;
            }
        }
        [Serializable]
        public class ValidatePermanentOpponentControls : ITargetValidator
        {
            private readonly CardType type;

            public ValidatePermanentOpponentControls(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Game game, Player player, Card source, GameObject target)
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
        public class ValidatePermanent : ITargetValidator
        {
            private readonly CardType type;

            public ValidatePermanent(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Game game, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;


                    if (card.isType(type) && card.Zone == Zone.Battlefield && card.CanBeTargeted(player, source))
                        return TargetValidation.Valid;


                    return TargetValidation.Invalid;
                }
                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class ValidateCreature : ValidatePermanent
        {
            public ValidateCreature() : base(CardType.Creature)
            {
            }
        }

        [Serializable]
        public class ValidatePlayer : ITargetValidator
        {
            public TargetValidation Validate(Game game, Player player, Card source, GameObject target)
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

    internal abstract class TargetPopulator
    {
        public GameObject Populate(Game game, Player player, Card source, List<ITargetValidator> validators, List<GameObject> selected)
        {
            var list = Populate(game, player, source);
            list = list.Where(x => validators.Validate(game, player, source, x) == TargetValidation.Valid);
            list = list.Where(x => !selected.Contains(x)).ToList();
            if (list.Count() == 0)
                return null;
            return player.request.RequestFromObjects(RequestType.SelectTarget, $"{player}, Select target for {source}", list);
        }

        public abstract IEnumerable<GameObject> Populate(Game game, Player player, Card source);
    }

    internal class TargetPopulators
    {
        [Serializable]
        public class TargetPermanentYouControl : TargetPopulator
        {
            private readonly CardType type;

            public TargetPermanentYouControl(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public override IEnumerable<GameObject> Populate(Game game, Player player, Card source)
            {
                return game.Methods.GetAllTargets(TargetType.Card)
                        .Where(o => ((Card)o).isType(type) && ((Card)o).Controller == player);
            }
        }

        [Serializable]
        public class TargetPermanentOpponentControls : TargetPopulator
        {
            private readonly CardType type;

            public TargetPermanentOpponentControls(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public override IEnumerable<GameObject> Populate(Game game, Player player, Card source)
            {
                return game.Methods.GetAllTargets(TargetType.Card)
                        .Where(o => ((Card) o).isType(type) && ((Card) o).Controller != player);
            }
        }

        [Serializable]
        public class TargetPermanent : TargetPopulator
        {
            private readonly CardType type;

            public TargetPermanent(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public override IEnumerable<GameObject> Populate(Game game, Player player, Card source)
            {
                return game.Methods.GetAllTargets(TargetType.Card).Where(o => ((Card) o).isType(type));
            }
        }

        [Serializable]
        public class TargetCreature : TargetPermanent
        {
            public TargetCreature() : base(CardType.Creature)
            {
            }
        }

        [Serializable]
        public class TargetPlayer : TargetPopulator
        {
            public override IEnumerable<GameObject> Populate(Game game, Player player, Card source)
            {
                return game.Methods.GetAllTargets(TargetType.Player);
            }
        }

        [Serializable]
        public class TargetCreatureOrPlayer : TargetPopulator
        {
            public override IEnumerable<GameObject> Populate(Game game, Player player, Card source)
            {
                var list = game.Methods.GetAllTargets(TargetType.Card | TargetType.Player).Where(o =>
                {
                    if (o is Card)
                        return ((Card) o).isType(CardType.Creature);
                    return true;
                });

                return list;
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
                Validators.Add(new TargetValidator.ValidatePermanent(type));
                Populator = new TargetPopulators.TargetPermanent(type);
            }
        }

        [Serializable]
        public class PermanentOpponentControls : AbstractTarget
        {
            public PermanentOpponentControls(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.ValidatePermanentOpponentControls(type));
                Populator = new TargetPopulators.TargetPermanentOpponentControls(type);
            }
        }

        [Serializable]
        public class PermanentYouControl : AbstractTarget
        {
            public PermanentYouControl(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.ValidatePermanentYouControl(type));
                Populator = new TargetPopulators.TargetPermanentYouControl(type);
            }
        }

        [Serializable]
        public class Creature : AbstractTarget
        {
            public Creature(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.ValidatePermanent(CardType.Creature));
                Populator = new TargetPopulators.TargetPermanent(CardType.Creature);
            }
        }

        [Serializable]
        public class Player : AbstractTarget
        {
            public Player(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.ValidatePlayer());
                Populator = new TargetPopulators.TargetPlayer();
            }
        }

        [Serializable]
        public class CreatureOrPlayer : AbstractTarget
        {
            public CreatureOrPlayer(Func<GameObject, bool> filter = null)
                : base(filter)
            {
                Validators.Add(new TargetValidator.ValidateCreature());
                Validators.Add(new TargetValidator.ValidatePlayer());
                Populator = new TargetPopulators.TargetCreatureOrPlayer();
            }
        }
    }

    #endregion
}