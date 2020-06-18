using System;
using NUnit.Framework;

namespace ImTools.Experimental.Tree234.UnitTests
{
    [TestFixture]
    public class ImMap234Tests
    {
        [Test]
        public void Adding_keys_from_1_to_10_and_checking_the_tree_shape_on_each_addition()
        {
            var m = ImMap<int>.Empty;
            Assert.AreEqual(default(int), m.GetValueOrDefault(0));
            Assert.AreEqual(default(int), m.GetValueOrDefault(13));

            m = m.AddOrUpdate(1, 1);
            Assert.IsInstanceOf<ImMap<int>.Entry>(m);
            Assert.AreEqual(1, m.GetValueOrDefault(1));

            m = m.AddOrUpdate(2, 2);
            Assert.IsInstanceOf<ImMap<int>.Leaf2>(m);
            Assert.AreEqual(2, m.GetValueOrDefault(2));

            m = m.AddOrUpdate(3, 3);
            Assert.IsInstanceOf<ImMap<int>.Leaf3>(m);
            Assert.AreEqual(3, m.GetValueOrDefault(3));

            m = m.AddOrUpdate(4, 4);
            Assert.IsInstanceOf<ImMap<int>.Leaf4>(m);
            Assert.AreEqual(4, m.GetValueOrDefault(4));
            Assert.AreEqual(3, m.GetValueOrDefault(3));
            Assert.AreEqual(2, m.GetValueOrDefault(2));
            Assert.AreEqual(1, m.GetValueOrDefault(1));

            m = m.AddOrUpdate(5, 5);
            Assert.IsInstanceOf<ImMap<int>.Leaf5>(m);
            Assert.AreEqual(5, m.GetValueOrDefault(5));

            m = m.AddOrUpdate(6, 6);
            Assert.IsInstanceOf<ImMap<int>.Branch2>(m);
            Assert.IsNotInstanceOf<ImMap<int>.Branch3>(m);
            Assert.IsInstanceOf<ImMap<int>.Leaf2>(((ImMap<int>.Branch2)m).Br0);
            Assert.IsInstanceOf<ImMap<int>.Leaf3>(((ImMap<int>.Branch2)m).Br1);
            Assert.AreEqual(3, m.GetValueOrDefault(3));
            Assert.AreEqual(5, m.GetValueOrDefault(5));
            Assert.AreEqual(6, m.GetValueOrDefault(6));

            m = m.AddOrUpdate(7, 7);
            Assert.IsInstanceOf<ImMap<int>.Branch2>(m);
            Assert.IsNotInstanceOf<ImMap<int>.Branch3>(m);
            Assert.AreEqual(7, m.GetValueOrDefault(7));

            m = m.AddOrUpdate(8, 8);
            Assert.IsInstanceOf<ImMap<int>.Branch2>(m);
            Assert.AreEqual(8, m.GetValueOrDefault(8));

            m = m.AddOrUpdate(9, 9);
            Assert.AreEqual(9, m.GetValueOrDefault(9));

            m = m.AddOrUpdate(10, 10);
            Assert.AreEqual(10, m.GetValueOrDefault(10));
        }

        [Test]
        public void Adding_1000_keys_and_randomly_checking()
        {
            var m = ImMap<int>.Empty;
            for (var i = 0; i < 1000; i++)
            {
                m = m.AddOrUpdate(i, i);
            }

            Assert.AreEqual(1, m.GetValueOrDefault(1));
            Assert.AreEqual(0, m.GetValueOrDefault(0));
            Assert.AreEqual(13, m.GetValueOrDefault(13));
            Assert.AreEqual(66, m.GetValueOrDefault(66));
            Assert.AreEqual(555, m.GetValueOrDefault(555));
            Assert.AreEqual(333, m.GetValueOrDefault(333));
            Assert.AreEqual(999, m.GetValueOrDefault(999));

            // non-existing keys 
            Assert.AreEqual(0, m.GetValueOrDefault(1000));
            Assert.AreEqual(0, m.GetValueOrDefault(-1));
        }

        [Test]
        public void Adding_1000_keys_descending_and_randomly_checking()
        {
            var m = ImMap<int>.Empty;
            for (var i = 1000 - 1; i >= 0; i--)
            {
                m = m.AddOrUpdate(i, i);
            }

            Assert.AreEqual(1, m.GetValueOrDefault(1));
            Assert.AreEqual(0, m.GetValueOrDefault(0));
            Assert.AreEqual(13, m.GetValueOrDefault(13));
            Assert.AreEqual(66, m.GetValueOrDefault(66));
            Assert.AreEqual(555, m.GetValueOrDefault(555));
            Assert.AreEqual(333, m.GetValueOrDefault(333));
            Assert.AreEqual(999, m.GetValueOrDefault(999));

            // non-existing keys 
            Assert.AreEqual(0, m.GetValueOrDefault(1000));
            Assert.AreEqual(0, m.GetValueOrDefault(-1));
        }

        [Test]
        public void Adding_1000_keys_randomly_and_randomly_checking()
        {
            var rnd = new Random();

            var m = ImMap<int>.Empty;
            for (var i = 0; i < 100; i++)
            {
                var n = rnd.Next(0, 1000);
                m = m.AddOrUpdate(n, n);
                Assert.AreEqual(n, m.GetValueOrDefault(n));
            }

            // non-existing keys 
            Assert.AreEqual(0, m.GetValueOrDefault(1000));
            Assert.AreEqual(0, m.GetValueOrDefault(-1));
        }

    }
}