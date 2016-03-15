using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Delver
{
    [Serializable]
    internal enum AbiltiyType
    {
        Activated,
        Effect,
        Cost,
        ActiveEffect
    }


    [Serializable]
    internal class Ability
    {
        public List<LayeredEffect> layeredEffects = new List<LayeredEffect>();
        public List<Cost> costs = new List<Cost>();
        public List<Effect> effects = new List<Effect>();

        public Ability(AbiltiyType type)
        {
            this.type = type;
            IsManaSource = false;
        }

        public Ability(Cost cost)
            : this(AbiltiyType.Cost)
        {
        }

        public Ability(Ability ability)
            : this(AbiltiyType.Activated)
        {
            child = ability;
        }

        public Ability(IEnumerable<Effect> effects)
            : this(AbiltiyType.Effect)
        {
            this.effects = effects.ToList();
        }

        public Ability(Effect effect)
            : this(AbiltiyType.Effect)
        {
            effects.Add(effect);
        }

        public Ability(LayeredEffect activeEffect)
            : this(AbiltiyType.ActiveEffect)
        {
            layeredEffects.Add(activeEffect);
        }

        public AbiltiyType type { get; set; }
        public Ability child { get; set; }

        public string Text { get; set; }

        public bool IsManaSource { get; set; }

        public PopulateResult Populate(Game game, Player player, Card source)
        {
            List<PopulateResult> results = new List<PopulateResult>();
            foreach (var effect in effects)
                if (effect.HasTargets)
                    results.Add(effect.Populate(game, player, source));

            if (results.Contains(PopulateResult.NoneSelected))
                return PopulateResult.NoneSelected;

            if (results.Contains(PopulateResult.NoLegalTargets))
                return PopulateResult.NoLegalTargets;

            return PopulateResult.Accepted;
        }

        public List<TargetValidation> Validate(Game game, Player player, Card source)
        {
            var list = new List<TargetValidation>();
            foreach (var effect in effects)
                list.AddRange(effect.Validate(game, player, source));
            return list;
        }

        public bool CanPay(Game game, Player player, Card source)
        {
            foreach (var cost in costs)
                if (!cost.CanPay(game, player, source))
                    return false;
            return true;
        }

        public override string ToString()
        {
            if (Text == null)
                return string.Join(", ", costs.Select(x => x.ToString())) + ": " +
                       string.Join(", ", effects.Select(x => x.ToString()));
            return Text;
        }
    }

    [Serializable]
    internal abstract class Effect
    {
        public Effect()
        {
        }

        public bool HasTargets { get { return _targets.Any() || AnyNymberOfTargets; } }

        private List<ITarget> _targets = new List<ITarget>();

        public void AddTarget(params ITarget[] targets)
        {
            foreach (var target in targets)
                this._targets.Add(target);
        }

        public List<GameObject> Targets
        {
            get
            {
                return _targets.Where(x => x.target != null).Select(x => x.target).ToList();
            }
        }

        AbstractTarget multipleTargetType;
        public bool AnyNymberOfTargets { get; set; }
        public void SetMultipleTargets(AbstractTarget target)
        {
            AnyNymberOfTargets = true;
            multipleTargetType = target;
        }

        public string Text { get; set; }

        public void PerformEffect(BaseEventInfo info, Card source)
        {
            var e = info.Clone(source);
            e.Targets = this.Targets;

            if (_targets.Any())
            {
                if (Validate(e.Game, e.sourcePlayer, e.sourceCard).Any(x => x == TargetValidation.Valid))
                {
                    Invoke(e);
                }
                else
                {
                    e.Game.PostData($"{e.sourceCard} effect with invalid target: {this}");
                }
            }
            else
            {
                Invoke(e);
            }
        }

        public abstract void Invoke(BaseEventInfo info);    

        public override string ToString()
        {
            if (Text == null)
                return GetType().Name;
            return Text;
        }

        public PopulateResult Populate(Game game, Player player, Card source)
        {
            var selected = new List<GameObject>();
            List<PopulateResult> results = new List<PopulateResult>();
            if (AnyNymberOfTargets)
            {
                var result = PopulateResult.Accepted;
                while (result != PopulateResult.NoneSelected && result != PopulateResult.NoLegalTargets)
                {
                    var t = multipleTargetType.Clone;
                    result = t.Populate(game, player, source, selected);
                    if (result == PopulateResult.Accepted)
                    {
                        _targets.Add(t);
                        selected.Add(t.target);
                    }
                }
            }
            else {
                foreach (var t in _targets)
                {
                    results.Add(t.Populate(game, player, source, selected));
                    selected.Add(t.target);
                }
            }

            if (results.Contains(PopulateResult.NoneSelected))
                return PopulateResult.NoneSelected;

            if (results.Contains(PopulateResult.NoLegalTargets))
                return PopulateResult.NoLegalTargets;

            return PopulateResult.Accepted;
        }

        public List<TargetValidation> Validate(Game game, Player player, Card source)
        {
            var list = new List<TargetValidation>();
            foreach (var t in _targets)
                list.Add(t.ValidationStatus(game, player, source));
            return list;
        }
    }

    [Serializable]
    internal abstract class Cost
    {
        public string Text { get; set; }

        public override string ToString()
        {
            if (Text == null)
                return GetType().Name;
            return Text;
        }

        public abstract bool TryToPay(Game game, Player activatingPlayer, Card source);

        public virtual bool CanPay(Game game, Player activatingPlayer, Card source)
        {
            return true;
        }
    }


    [Serializable]
    internal class Abilities : IEnumerable<Ability>
    {
        private readonly List<Ability> _abilities = new List<Ability>();

        public Abilities()
        {
        }

        public Abilities(IEnumerable<Effect> effects)
        {
            _abilities.Add(new Ability(effects));
        }


        IEnumerator<Ability> IEnumerable<Ability>.GetEnumerator()
        {
            return _abilities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _abilities.GetEnumerator();
        }

        public void Add(Ability ability)
        {
            _abilities.Add(ability);
        }

        public void Add(Effect effect)
        {
            _abilities.Add(new Ability(effect));
        }


        public void Add(LayeredEffect layeredEffect)
        {
            _abilities.Add(new Ability(layeredEffect));
        }

        public bool Validate(Game game, Player player, Card card)
        {
            var list = new List<TargetValidation>();
            foreach (var ability in _abilities)
                list.AddRange(ability.Validate(game, player, card));

            if (list.Count == 0)
                return true;

            if (list.All(x => x == TargetValidation.Invalid))
                return false;

            return true;
        }


        public bool CanPay(Game game, Player player, Card card)
        {
            foreach (var ability in _abilities)
                if (!ability.CanPay(game, player, card))
                    return false;
            return true;
        }
    }
}