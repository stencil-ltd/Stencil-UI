using System;

namespace State.Containers
{
    public interface IStateHaver<T> : INotifiable
    {
        event EventHandler<StateTransition<T>> OnChange;
        T Value { get; set; }
    }
}