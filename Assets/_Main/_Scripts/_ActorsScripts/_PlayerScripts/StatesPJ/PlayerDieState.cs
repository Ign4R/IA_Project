using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState<T> : EntityStateBase<T>
{   
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
    }
    public override void Awake()
    { 
        base.Awake();
        _model.Move(Vector3.zero);
        _model.LookDir(Vector3.zero);
        GameManager.Instance.GameOver();

    }


}


