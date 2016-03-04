using System;
using Delver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTestMana
    {
        [TestMethod]
        public void TestMana()
        {
            var g = new Mana(Identity.Green);
            Assert.IsTrue(g.Color == Identity.Green);
            Assert.IsTrue(g.ToString() == "{G}");
        }

        [TestMethod]
        public void TestManaContains()
        {
            var pool = new ManaCost("GGG");
            Assert.IsTrue(pool.Contains(new ManaCost("1")));
            Assert.IsTrue(pool.Contains(new ManaCost("2")));
            Assert.IsTrue(pool.Contains(new ManaCost("3")));
            Assert.IsTrue(pool.Contains(new ManaCost("G")));
            Assert.IsTrue(pool.Contains(new ManaCost("GG")));
            Assert.IsTrue(pool.Contains(new ManaCost("GGG")));
            Assert.IsTrue(pool.Contains(new ManaCost("1GG")));
            Assert.IsTrue(pool.Contains(new ManaCost("2G")));
        }

        [TestMethod]
        public void TestManaContainsSimple()
        {
            var pool = new ManaCost("GGG");
            Assert.IsTrue(pool.Contains(new ManaCost("1")));
            Assert.IsTrue(pool.Contains(new ManaCost("2")));
            Assert.IsTrue(pool.Contains(new ManaCost("3")));
        }

        [TestMethod]
        public void TestManaContainsSimple2()
        {
            var pool = new ManaCost("GGG");

            Assert.IsTrue(pool.Contains(new ManaCost("G")));
            Assert.IsTrue(pool.Contains(new ManaCost("GG")));
            Assert.IsTrue(pool.Contains(new ManaCost("GGG")));
            Assert.IsTrue(pool.Contains(new ManaCost("1GG")));
            Assert.IsTrue(pool.Contains(new ManaCost("2G")));
        }


        [TestMethod]
        public void TestManaContainsNot()
        {
            var pool = new ManaCost("GGG");
            Assert.IsFalse(pool.Contains(new ManaCost("W")));
            Assert.IsFalse(pool.Contains(new ManaCost("U")));
            Assert.IsFalse(pool.Contains(new ManaCost("B")));
            Assert.IsFalse(pool.Contains(new ManaCost("R")));
            Assert.IsFalse(pool.Contains(new ManaCost("C")));
        }


        [TestMethod]
        public void TestManaContainsNot2()
        {
            var pool = new ManaCost("GGG");
            var cost = new ManaCost("4");
            Assert.IsFalse(pool.Contains(cost));
        }


        [TestMethod]
        public void TestManPay()
        {
            var pool = new ManaCost("GGG");
            var cost = new ManaCost("1G");
            var used = pool.UsedToPay(cost);
            Assert.IsTrue(used.ToString() == "{G}{G}");
        }

        [TestMethod]
        public void TestManPayReminder()
        {
            var pool = new ManaCost("GGG");
            var cost = new ManaCost("1G");
            var reminder = pool.Reminder(cost);
            Assert.IsTrue(reminder.ToString() == "{G}");
        }

        [TestMethod]
        public void TestManPayReminder2()
        {
            var pool = new ManaCost("GG");
            var cost = new ManaCost("1G");
            var reminder = pool.Reminder(cost);
            Assert.IsTrue(reminder.ToString() == "");
        }


        [TestMethod]
        public void TestManPayColored()
        {
            var pool = new ManaCost("RG");
            var cost = new ManaCost("1G");
            var used = pool.UsedToPay(cost);
            var reminder = pool.Reminder(cost);
            Assert.IsTrue(reminder.ToString() == "");
        }

        [TestMethod]
        public void TestManPayColored2()
        {
            var pool = new ManaCost("GR");
            var cost = new ManaCost("1G");
            var used = pool.UsedToPay(cost);
            var reminder = pool.Reminder(cost);
            Assert.IsTrue(reminder.ToString() == "");
        }

        [TestMethod]
        [ExpectedException(typeof (Exception), "Invalid mana used")]
        public void TestManPay2()
        {
            var pool = new ManaCost("RRR");
            var cost = new ManaCost("1G");
            var used = pool.UsedToPay(cost);
            var reminder = pool.Reminder(cost);
        }


        [TestMethod]
        public void TestManPayMany()
        {
            var pool = new ManaCost("WUBRGCGGCC");
            var cost = new ManaCost("WUBRGC1");
            var reminder = pool.Reminder(cost);
            Assert.IsTrue(reminder.ToString() == "{G}{C}{C}");
        }
    }
}