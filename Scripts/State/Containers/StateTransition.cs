using System.Collections.Generic;

namespace State.Containers
{
    public struct StateTransition<T>
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public StateTransition(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public bool Equals(StateTransition<T> other)
        {
            return EqualityComparer<T>.Default.Equals(OldValue, other.OldValue) && EqualityComparer<T>.Default.Equals(NewValue, other.NewValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StateTransition<T> && Equals((StateTransition<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(OldValue) * 397) ^ EqualityComparer<T>.Default.GetHashCode(NewValue);
            }
        }
    }
}