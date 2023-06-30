using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : NavigationState<T> 
{
    ISteering _steering;


    public EnemyStateChase(ISteering steering)
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
        if (_steering != null)
        {
            Vector3 avoidance = _enemyModel.ObsAvoidance.GetDir().normalized;
            Debug.Log(avoidance + " soy OBSAVOID");
            Debug.Log(avoidance.magnitude + " soy magnitud avoid");
            Vector3 dirTarget = _steering.GetDir().normalized;
            var dirFinal = (dirTarget + avoidance.normalized * _enemyModel._multiplierAvoid).normalized;
            _model.Move(dirFinal);
            _model.LookDir(dirFinal);
        }

    }




    public override void Sleep()
    {
        base.Sleep();
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;

    }
}
