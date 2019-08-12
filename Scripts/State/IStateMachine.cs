namespace State
{
    public interface IStateMachine
    {
        void ResetState(bool clearPersistence = true);
    }
}