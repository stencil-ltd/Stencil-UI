namespace State
{
    public struct StateChange<T> where T : struct
    {
        public readonly T? Old;
        public readonly T New;

        public StateChange(T? old, T @new)
        {
            Old = old;
            New = @new;
        }
    }
}