using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : NavigationState<T> 
{
    ISteering _steering;
    EnemyModel _enemyModel;
    public EnemyStateChase(ISteering steering)
    {
        _steering = steering;
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyModel = (EnemyModel)model;
    }
    public override void Awake()
    {
        
        _enemyModel.SpottedTarget = true;
        base.Awake();
        _enemyModel._coneOfView.color = Color.red;
        _model.OnRun += _view.AnimRun;
    }

    public override void Execute()
    {
        Debug.Log("Chase Execute");
        base.Execute();
        if (_steering != null)
        {
            Vector3 dirTarget = _steering.GetDir().normalized;
            _model.Move(dirTarget);
            _model.LookDir(dirTarget);
        }

    }

    public override void Sleep()
    {
        base.Sleep();
        Debug.Log("Chase Sleep");
        //_model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;

    }
}
