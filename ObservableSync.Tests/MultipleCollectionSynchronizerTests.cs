using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObservableSync.Tests
{
    [TestFixture]
    public class MultipleCollectionSynchronizerTests
    {
        private ObservableCollection<string> _destination;
        private ObservableCollectionSynchronizer<string> _sync;
        private ObservableCollection<int> _source1;
        private Func<int, string> _source1Selector;
        private ObservableCollection<string> _source2;

        [SetUp]
        public void Setup()
        {
            _destination = new ObservableCollection<string>();
            _sync = new ObservableCollectionSynchronizer<string>(_destination);
            _source1 = new ObservableCollection<int>();
            _source2 = new ObservableCollection<string>();
            _source1Selector = (i => i.ToString());
        }

        [Test]
        public void CatchNullDestination()
        {
            Assert.Throws<ArgumentNullException>(() => new ObservableCollectionSynchronizer<string>(null));
        }

        [Test]
        public void CatchEmptySource()
        {
            Assert.Throws<ArgumentNullException>(() => _sync.Synchronize(null));
        }

        [Test]
        public void CanAddEmptyLists()
        {
            _sync.Synchronize(_source1, _source1Selector);
            _sync.Synchronize(_source2);

            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanAddListWithElementInEachSource()
        {
            _source1.Add(1);
            _source2.Add("2");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleAdd()
        {
            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1.Add(1);
                _source2.Add("2");
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeRemoveAtEndOfEachSource()
        {
            _source1.Add(1);
            _source1.Add(2);
            _source2.Add("3");
            _source2.Add("4");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1.Remove(2);
                _source2.Remove("4");
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        public void CanSynchronizeRemoveAtBeginningOfEachSource()
        {
            _source1.Add(1);
            _source1.Add(2);
            _source2.Add("3");
            _source2.Add("4");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1.Remove(1);
                _source2.Remove("3");
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleMoveFromEachSource()
        {
            _source1.Add(1);
            _source1.Add(2);
            _source2.Add("3");
            _source2.Add("4");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1.Move(1, 0);
                _source2.Move(1, 0);
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleReplaceFromEachSource()
        {
            _source1.Add(1);
            _source2.Add("3");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1[0] = 2;
                _source2[0] = "4";
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeClear()
        {
            _source1.Add(1);
            _source1.Add(2);
            _source1.Add(3);
            _source2.Add("1");
            _source2.Add("2");
            _source2.Add("3");

            using (_sync.Synchronize(_source1, _source1Selector))
            using (_sync.Synchronize(_source2))
            {
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
                _source1.Clear();
                _source2.Clear();
                Assert.That(_destination, Is.EqualTo(_source1.Select(_source1Selector).Concat(_source2)).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }
    }
}
