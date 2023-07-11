using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T>
{
    Dictionary<T, IState<T>> _transitions = new Dictionary<T, IState<T>>();
    public virtual void Awake()
    {
        
    }

    public virtual void Execute()
    {


    }

    public virtual void Sleep()
    {

    }
    
    public void AddTransition(T input, IState<T> state)
    {
        _transitions[input] = state;
    }
    public  void RemoveTransition(T input)
    {
        if (_transitions.ContainsKey(input))
            _transitions.Remove(input);
    }

    public IState<T> GetTransition(T input)
    {
        if (_transitions.ContainsKey(input))
            return _transitions[input];
        return null;
    }
}
