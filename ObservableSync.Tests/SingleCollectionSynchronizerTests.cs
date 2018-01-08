using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace ObservableSync.Tests
{
    [TestFixture]
    public class SingleCollectionSynchronizerTests
    {
        private ObservableCollection<string> _destination;
        private ObservableCollectionSynchronizer<string> _sync;
        private ObservableCollection<string> _source;

        [SetUp]
        public void Setup()
        {
            _destination = new ObservableCollection<string>();
            _sync = new ObservableCollectionSynchronizer<string>(_destination);
            _source = new ObservableCollection<string>();
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
        public void CanAddEmptyList()
        {
            _sync.Synchronize(_source);

            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanAddListWithElement()
        {
            _source.Add("1");

            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleAdd()
        {
            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
                _source.Add("1");
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleRemove()
        {
            _source.Add("1");
            _source.Add("2");

            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
                _source.Remove("2");
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleMove()
        {
            _source.Add("1");
            _source.Add("2");

            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
                _source.Move(1, 0);
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeSingleReplace()
        {
            _source.Add("1");

            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
                _source[0] = "2";
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }

        [Test]
        public void CanSynchronizeClear()
        {
            _source.Add("1");
            _source.Add("2");
            _source.Add("3");

            using (_sync.Synchronize(_source))
            {
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
                _source.Clear();
                Assert.That(_destination, Is.EqualTo(_source).AsCollection);
            }
            Assert.That(_destination.Count, Is.Zero);
        }
    }
}
