using System;
using UnityEngine;

public class ReentryState<T> : EntityStateBase<T>
{
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
    }
    public override void Awake()
    {
     
        base.Awake();
        _fsm.RepeatState(true);
    }
    public override void Execute()
    {
        base.Execute();
 
   
    }
    public override void Sleep()
    {
        base.Sleep();
        _fsm.RepeatState(false);


    }
}