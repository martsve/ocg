using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("UnitTest")]

namespace Delver
{
    internal class ManaCostException : Exception
    {
        public ManaCostException()
        {
        }

        public ManaCostException(string message)
            : base(message)
        {
        }

        public ManaCostException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ManaCostException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }


    [Serializable]
    internal class ManaCost
    {
        protected List<Mana> Cost = new List<Mana>();


        public ManaCost()
        {
        }

        public ManaCost(string mana)
        {
            SetMana(mana);
        }

        public ManaCost(List<Mana> mana)
        {
            Add(mana);
        }

        public ManaCost(Mana mana)
        {
            Add(mana);
        }

        public int Count
        {
            get { return Cost.Count(); }
        }

        #region Implementation of IEnumerable

        public IEnumerator<Mana> GetEnumerator()
        {
            return Cost.OrderByDescending(x => x.priority).GetEnumerator();
        }

        #endregion

        public void Add(List<Mana> mana)
        {
            Cost.AddRange(mana);
        }

        public void Add(Mana mana)
        {
            Cost.Add(mana);
        }

        public void Add(Mana mana, int N)
        {
            for (var i = 0; i < N; i++)
            {
                Cost.Add(mana);
            }
        }

        public Identity getIdentity()
        {
            var identity = Identity.None;
            var colors =
                Cost.Where(m => m.Color != Identity.Colorless && m.Color != Identity.Generic)
                    .Select(m => m.Color)
                    .Distinct()
                    .ToList();

            foreach (var color in colors)
                identity = identity | color;

            if (identity == Identity.None)
                identity = Identity.Colorless;

            return identity;
        }

        public bool Contains(Identity color)
        {
            return Cost.Any(m => m.Color == color);
        }

        public bool Contains(Mana mana)
        {
            if (Find(mana) != null)
                return true;
            return false;
        }

        public bool Contains(ManaCost mana)
        {
            var pool = new ManaCost(Cost);
            foreach (var m in mana)
            {
                if (!pool.Contains(m))
                    return false;
                pool.Remove(m);
            }
            return true;
        }

        public ManaCost Reminder(ManaCost mana)
        {
            var cost = new ManaCost(Cost);
            foreach (var m in mana)
                cost.Remove(m);
            return cost;
        }

        public ManaCost UsedToPay(ManaCost mana)
        {
            var used = new ManaCost();
            var cost = new ManaCost(Cost);
            foreach (var m in mana)
            {
                var u = cost.Find(m);
                if (u == null)
                    throw new Exception("Invalid mana used");
                cost.RemoveExactly(u);
                used.Add(u);
            }
            return used;
        }

        public Mana Find(Mana mana)
        {
            var found = Cost.FirstOrDefault(m => m.Color == mana.Color);
            if (found == null && mana.Color == Identity.Generic && Cost.Any())
            {
                found = Cost.OrderBy(x => x.priority).FirstOrDefault();
            }
            return found;
        }

        public void Remove(ManaCost mana)
        {
            foreach (var m in mana)
                Remove(m);
        }

        public void Remove(Mana mana)
        {
            RemoveExactly(Find(mana));
        }

        public bool ContainsExactly(Mana mana)
        {
            return Cost.Contains(mana);
        }

        public void RemoveExactly(Mana m)
        {
            Cost.Remove(m);
        }

        public void RemoveExactly(ManaCost mana)
        {
            foreach (var m in mana)
                Cost.Remove(m);
        }

        public void Clear()
        {
            Cost.Clear();
        }

        private void SetMana(string mana)
        {
            while (mana.Length > 0)
            {
                var match = Regex.Match(mana, @"^([0-9]+|\{.*?\}|[WUBRGC])");
                if (match.Success)
                {
                    var s = match.Value.Trim('{', '}');

                    switch (s)
                    {
                        case "W":
                            Add(new Mana(Identity.White));
                            break;
                        case "U":
                            Add(new Mana(Identity.Blue));
                            break;
                        case "B":
                            Add(new Mana(Identity.Black));
                            break;
                        case "R":
                            Add(new Mana(Identity.Red));
                            break;
                        case "G":
                            Add(new Mana(Identity.Green));
                            break;
                        case "C":
                            Add(new Mana(Identity.Colorless));
                            break;
                        default:
                            int N;
                            if (int.TryParse(s, out N))
                                Add(new Mana(Identity.Generic), N);
                            else
                                throw new ManaCostException("Invalid mana symbol: " + s);
                            break;
                    }

                    mana = mana.Substring(match.Value.Length);
                }
                else
                    throw new ManaCostException("Invalid casting cost: " + mana);
            }
        }

        public override string ToString()
        {
            var result = "";
            foreach (var m in Cost.OrderBy(x => x.priority).ThenBy(x => x.Color))
                result += m.ToString();
            return result;
        }
    }

    [Serializable]
    internal class Mana : GameObject
    {
        public int priority;
        public string Special;

        public Mana(Identity color)
        {
            Color = color;

            if (Color == Identity.Generic)
                priority = 0;
            else
                priority = 1;
        }

        public Identity Color { get; set; }

        public override string ToString()
        {
            var color = "";
            if (Color == Identity.Blue)
                color = "U";
            if (Color == Identity.Generic)
                color = "1";
            else
                color = Color.ToString().Substring(0, 1);

            return "{" + color + "}";
        }
    }
}