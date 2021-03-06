﻿using System;
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
        PopulateResult Populate(Context Context, Player player, Card source, List<GameObject> selected);
        TargetValidation ValidationStatus(Context Context, Player player, Card source);
        TargetValidation Validate(Context Context, Player player, Card source, GameObject target);
    }

    [Serializable]
    internal abstract class AbstractTarget : ITarget
    {
        public TargetPopulator Populator;
        public List<ITargetValidator> Validators = new List<ITargetValidator>();

        public AbstractTarget()
        {
        }

        public AbstractTarget Clone => (AbstractTarget)this.MemberwiseClone();

        public GameObjectReferance _target { get; set; }

        public GameObject target => _target?.Object;

        public PopulateResult Populate(Context Context, Player player, Card source, List<GameObject> selected)
        {
            try
            {
                var obj = Populator.Populate(Context, player, source, Validators, selected);
                if (obj == null)
                    return PopulateResult.NoneSelected;
                _target = obj.Referance;
                return PopulateResult.Accepted;
            }
            catch (NoLegalTargetsException)
            {
                return PopulateResult.NoLegalTargets;
            }
        }

        public TargetValidation ValidationStatus(Context Context, Player player, Card source)
        {
            return Validate(Context, player, source, target);
        }

        public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
        {
            if (target == null)
                return TargetValidation.Invalid;

            return Validators.Validate(Context, player, source, target);
        }
    }

    static class TargetValidatorExtension
    {
        public static TargetValidation Validate(this List<ITargetValidator> validators, Context Context, Player player, Card source, GameObject target)
        {
            foreach (var validator in validators)
            {
                if (validator.Validate(Context, player, source, target) == TargetValidation.Invalid)
                    return TargetValidation.Invalid;
                if (validator.Validate(Context, player, source, target) == TargetValidation.NotSet)
                    return TargetValidation.NotSet;
            }
            return TargetValidation.Valid;
        } 
    }

    internal interface ITargetValidator
    {
        TargetValidation Validate(Context Context, Player player, Card source, GameObject target);
    }


    #region Target Validators

    [Serializable]
    internal class TargetValidator
    {
        [Serializable]
        public class ValidatePermanentYouControl : ITargetValidator
        {
            private readonly CardType type;

            public ValidatePermanentYouControl(CardType type = CardType.Permanent)
            {
                this.type = type;
            }

            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isCardType(type) && card.Zone == Zone.Battlefield && card.Controller == player)
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

            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isCardType(type) && card.Zone == Zone.Battlefield && card.Controller != player)
                        return TargetValidation.Valid;
                    return TargetValidation.Invalid;
                }
                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class ValidateFilter : ITargetValidator
        {
            private readonly Func<GameObject, bool> filter;

            public ValidateFilter(Func<GameObject, bool> filter)
            {
                this.filter = filter;
            }

            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (filter.Invoke(target))
                    return TargetValidation.Valid;
                else
                    return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class ValidateCard : ITargetValidator
        {
            private readonly CardType type;
            private readonly Zone zone;

            public ValidateCard(CardType type = CardType.Permanent, Zone zone = Zone.Graveyard)
            {
                this.type = type;
                this.zone = zone;
            }

            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;

                    if (card.isCardType(type) && card.Zone == zone && card.CanBeTargeted(player, source))
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

            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Card)
                {
                    var card = target as Card;


                    if (card.isCardType(type) && card.Zone == Zone.Battlefield && card.CanBeTargeted(player, source))
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
        public class ValidateCreatureOrPlayer : ITargetValidator
        {
            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Delver.Player && ((Delver.Player)target).IsPlaying)
                    return TargetValidation.Valid;

                if (target is Card)
                {
                    var card = target as Card;
                    if (card.isCardType(CardType.Creature) && card.Zone == Zone.Battlefield)
                        return TargetValidation.Valid;
                }

                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class ValidatePlayer : ITargetValidator
        {
            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Delver.Player && ((Delver.Player) target).IsPlaying)
                    return TargetValidation.Valid;
                return TargetValidation.Invalid;
            }
        }

        [Serializable]
        public class ValidateOpponent : ITargetValidator
        {
            public TargetValidation Validate(Context Context, Player player, Card source, GameObject target)
            {
                if (target is Delver.Player && ((Delver.Player)target).IsPlaying && target != player)
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
        public GameObject Populate(Context Context, Player player, Card source, List<ITargetValidator> validators, List<GameObject> selected)
        {
            var list = Populate(Context, player, source);
            list = list.Where(x => validators.Validate(Context, player, source, x) == TargetValidation.Valid);
            list = list.Where(x => !selected.Contains(x)).ToList();
            if (list.Count() == 0)
                throw new NoLegalTargetsException();
            return player.request.RequestFromObjects(MessageType.SelectTarget, $"{player}, Select target for {source}", list);
        }

        public abstract IEnumerable<GameObject> Populate(Context Context, Player player, Card source);
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

            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Card)
                        .Where(o => ((Card)o).isCardType(type) && ((Card)o).Controller == player);
            }
        }

        [Serializable]
        public class TargetCard : TargetPopulator
        {
            private readonly CardType type;
            private readonly Zone zone;
            private readonly Player player;
            public TargetCard(CardType type = CardType.Permanent, Zone zone = Zone.Graveyard, Player player = null)
            {
                this.type = type;
                this.zone = zone;
                this.player = player;
            }

            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Card, player, zone).Where(o => ((Card)o).isCardType(type));
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

            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Card)
                        .Where(o => ((Card) o).isCardType(type) && ((Card) o).Controller != player);
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

            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Card).Where(o => ((Card) o).isCardType(type));
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
            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Player);
            }
        }

        [Serializable]
        public class TargetOpponent : TargetPopulator
        {
            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                return Context.Methods.GetAllTargets(TargetType.Player).Where(x => x != player);
            }
        }

        [Serializable]
        public class TargetCreatureOrPlayer : TargetPopulator
        {
            public override IEnumerable<GameObject> Populate(Context Context, Player player, Card source)
            {
                var list = Context.Methods.GetAllTargets(TargetType.Card | TargetType.Player).Where(o =>
                {
                    if (o is Card)
                        return ((Card) o).isCardType(CardType.Creature);
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
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidatePermanent(type));
                Populator = new TargetPopulators.TargetPermanent(type);
            }
        }

        [Serializable]
        public class CreatureCard : AbstractTarget
        {
            public CreatureCard(CardType type = CardType.Creature, Func<GameObject, bool> filter = null, Zone zone = Zone.Graveyard)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidateCard(type, zone));
                Populator = new TargetPopulators.TargetCard(type, zone);
            }
        }

        [Serializable]
        public class PermanentOpponentControls : AbstractTarget
        {
            public PermanentOpponentControls(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidatePermanentOpponentControls(type));
                Populator = new TargetPopulators.TargetPermanentOpponentControls(type);
            }
        }

        [Serializable]
        public class PermanentYouControl : AbstractTarget
        {
            public PermanentYouControl(CardType type = CardType.Permanent, Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidatePermanentYouControl(type));
                Populator = new TargetPopulators.TargetPermanentYouControl(type);
            }
        }

        [Serializable]
        public class Creature : AbstractTarget
        {
            public Creature(Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidatePermanent(CardType.Creature));
                Populator = new TargetPopulators.TargetPermanent(CardType.Creature);
            }
        }

        [Serializable]
        public class Player : AbstractTarget
        {
            public Player(Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidatePlayer());
                Populator = new TargetPopulators.TargetPlayer();
            }
        }

        [Serializable]
        public class Opponent : AbstractTarget
        {
            public Opponent(Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidateOpponent());
                Populator = new TargetPopulators.TargetOpponent();
            }
        }

        [Serializable]
        public class CreatureOrPlayer : AbstractTarget
        {
            public CreatureOrPlayer(Func<GameObject, bool> filter = null)
            {
                if (filter != null)
                    Validators.Add(new TargetValidator.ValidateFilter(filter));
                Validators.Add(new TargetValidator.ValidateCreatureOrPlayer());
                Populator = new TargetPopulators.TargetCreatureOrPlayer();
            }
        }
    }

    #endregion
}