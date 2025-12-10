namespace Dave6.StatSystem.Interaction
{
    /// <summary>
    /// Character, NPC
    /// </summary>
    public interface IStatReceiver
    {
        void Accept(IStatInvoker invoker);
    }
}
