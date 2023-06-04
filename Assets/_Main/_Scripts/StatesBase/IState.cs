public interface IState<T>
{
    void Awake();
    void Execute();
    void Sleep();
    void AddTransition(T input, IState<T> state);
    void RemoveTransition(T input);
    IState<T> GetTransition(T input);
}

