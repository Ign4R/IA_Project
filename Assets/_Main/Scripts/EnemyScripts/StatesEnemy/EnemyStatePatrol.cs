using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{  
    public override void Awake()
    {       
        base.Awake();
        _model.OnRun += _view.AnimRun;
    }
    public override void Execute()
    {
        Debug.Log("Patrol State");
        base.Execute();
        Vector3 dir = Wp.GetDirWP();
        Wp.TouchWayPoint();
        _model.Move(dir);
        _model.LookDir(dir);
    }

    public override void Sleep()
    {
        base.Sleep();
    }
}
