using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDie<T> : EntityStateBase<T>
{   
    T _inputRunning;
    public PlayerStateDie(T inputRunning)
    {
        _inputRunning = inputRunning;
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
    }
    public override void Awake()
    { 
        base.Awake();
        _model.Move(Vector3.zero);
        _model.LookDir(Vector3.zero);
        _model.gameObject.layer = default;

        GameManager.Instance.GameOver();

    }


}


