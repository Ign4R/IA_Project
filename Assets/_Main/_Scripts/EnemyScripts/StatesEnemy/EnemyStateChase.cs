using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : NavigationState<T> 
{
    ISteering _steering;


    public EnemyStateChase(ISteering steering )
    {
        _steering = steering;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun;
    }
   
    public override void Execute()
    {
        Debug.Log("Chase State");
        base.Execute();
        var dir = _steering.GetDir();
        _model.Move(dir);
        _model.LookDir(dir);
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;

    }
}
