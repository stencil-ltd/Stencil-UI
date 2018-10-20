namespace State.Dynamic
{
    public struct DynamicStateChange
    {
        public readonly DynamicState Old;
        public readonly DynamicState New;

        public DynamicStateChange(DynamicState old, DynamicState @new)
        {
            Old = old;
            New = @new;
        }
    }
}