using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStateBase<T> : State<T>
{
    protected ISteering _steering;
    protected BaseModel _model;
    protected BaseView  _view;
    protected FSM<T> _fsm;
    public float CurrentTimer { get; protected set; }
    public virtual void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        _model = model;
        _fsm = fsm;
        _view = view;
    }
   
    protected int SetRandomTimer(float maxFloat)
    {
        int timer = Random.Range(1, (int)maxFloat);
        return timer;
    }

    public void Timer(float value = -1f)
    {
        CurrentTimer += value * Time.deltaTime;
    }

   
}
