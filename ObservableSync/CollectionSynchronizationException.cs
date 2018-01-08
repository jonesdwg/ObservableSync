using System;

namespace ObservableSync
{
    [Serializable]
    public class CollectionSynchronizationException : Exception
    {
        public CollectionSynchronizationException() { }
        public CollectionSynchronizationException(string message) : base(message) { }
        public CollectionSynchronizationException(string message, Exception inner) : base(message, inner) { }
        protected CollectionSynchronizationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
