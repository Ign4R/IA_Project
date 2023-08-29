public class FSM<T>
{
    IState<T> _current;
    public bool StateRepeat { get; private set; }
    public IState<T> Current { get => _current; set => _current = value; }


    public FSM() 
    {
    }
    public FSM(IState<T> init)
    {
        SetInit(init);
    }
    public void RepeatState(bool v)
    {
        StateRepeat = v;
    }
    public void SetInit(IState<T> init)
    {
        _current = init;
        _current.Awake();
    }
    public void OnUpdate()
    {
        if (_current != null)
            _current.Execute();    
    }
    public void Transitions(T input)
    {
        IState<T> newState = _current.GetTransition(input);
        if (newState == null && !input.Equals(default(T))) return;
        _current.Sleep();
        _current = newState;
        if (_current == null) return;
        _current.Awake();
    }


    ///TODO: Como si o si poner en sleep el estado actual, cuando input(o el state a transicionar) es default/nulo 
    /// Bueno creo que lo hize XD

}


