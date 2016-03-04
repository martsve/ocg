using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Delver
{
    internal class NoPlayersException : Exception
    {
        public NoPlayersException()
        {
        }

        public NoPlayersException(string message)
            : base(message)
        {
        }

        public NoPlayersException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NoPlayersException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    internal static class MyExtensions
    {
        public static string ToJson(this object obj)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
        public static void Shuffle<T>(this IList<T> list, Rand rnd)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Pop<T>(this List<T> list, int index = 0)
        {
            var r = list[index];
            list.RemoveAt(index);
            return r;
        }

        public static void Push<T>(this List<T> list, T item, int index = 0)
        {
            list.Insert(index, item);
        }

        public static bool isType(this TargetType type, TargetType ask)
        {
            return (type & ask) == ask;
        }


        public static bool IsSameOrSubclass(this Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }


        public static ReferanceList<GameObject> ToReferance(this IEnumerable<GameObject> list)
        {
            return new ReferanceList<GameObject>(list);
        }

        public static ReferanceList<Card> ToReferance(this IEnumerable<Card> list)
        {
            return new ReferanceList<Card>(list);
        }

        public static ReferanceList<Player> ToReferance(this IEnumerable<Player> list)
        {
            return new ReferanceList<Player>(list);
        }
    }

    [Serializable]
    internal class ReferanceList<T> : IEnumerable<T> where T : GameObject
    {
        private readonly List<GameObjectReferance> referances;

        public ReferanceList(IEnumerable<T> list)
        {
            referances = list.Select(x => x.Referance).ToList();
        }

        public ReferanceList(IEnumerable<GameObjectReferance> list)
        {
            referances = list.ToList();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var r in referances)
            {
                if (r.Object != null)
                    yield return (T) r.Object;
            }
        }

        public static implicit operator ReferanceList<T>(List<T> list)
        {
            return new ReferanceList<T>(list);
        }
    }
}