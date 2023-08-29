using System;
using UnityEngine;

public class AgainRepeatState<T> : EntityStateBase<T>
{

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);

    }

    public override void Awake()
    {
        base.Awake();
        Debug.Log("Entre en AGAINREPEAT STATE");
     
        _fsm.RepeatState(true);
    }
    public override void Sleep()
    {
        Debug.Log("Sali en AGAINREPEAT STATE");
        _fsm.RepeatState(false);
        base.Sleep();
        
    }

}