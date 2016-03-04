using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Delver
{
    [Serializable]
    internal class Rand
    {
        private static int seedGenCount = 1;
        private readonly Random rnd;
        public int Seed;

        private readonly UniqueRandom uniqueRandoms;

        public Rand(int Seed = -1)
        {
            if (Seed < 0)
                Seed = SeedGenerator.Next(int.MaxValue);
            rnd = new Random(Seed);
            uniqueRandoms = new UniqueRandom(Seed, 1, 1000);
        }

        private static Random SeedGenerator
        {
            get
            {
                return
                    new Random(unchecked(Environment.TickCount*31 + seedGenCount++*Thread.CurrentThread.ManagedThreadId));
            }
        }

        public int Next(int max)
        {
            return rnd.Next(max);
        }

        public int Unique()
        {
            return uniqueRandoms.Get();
        }

        [Serializable]
        private class UniqueRandom
        {
            private int from = 1;
            private readonly Random rnd;
            private int to = 10;
            private Queue<int> uniques;

            public UniqueRandom(int seed, int from, int to)
            {
                rnd = new Random(seed);
                SetRand(from, to);
            }

            private int GetRand(int from, int to)
            {
                return rnd.Next(from, to);
            }

            private void SetRand(int From, int To)
            {
                from = From;
                to = To;
                GenerateUnique(from, to);
            }

            private void GenerateUnique(int from, int to)
            {
                var result = Enumerable.Range(from, to).OrderBy(item => rnd.Next());
                uniques = new Queue<int>(result);
            }

            public int Get()
            {
                if (uniques == null)
                {
                    GenerateUnique(from, to);
                }
                var r = uniques.Dequeue();
                if (uniques.Count == 0)
                {
                    uniques = null;
                    from = to + 1;
                    to = to*2;
                }
                return r;
            }
        }
    }
}