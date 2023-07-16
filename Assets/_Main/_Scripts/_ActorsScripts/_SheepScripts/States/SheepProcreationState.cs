using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepProcreationState<T> : EntityStateBase<T>
{
    AllyModel _sheepModel;
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepModel = model as AllyModel;
    }
    public override void Awake()
    {
        base.Awake();
        _model.Move(Vector3.zero);
    }
    public override void Execute()
    {
        base.Execute();

    }

   
}
