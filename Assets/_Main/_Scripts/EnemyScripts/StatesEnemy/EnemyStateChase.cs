using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : NavigationState<T> 
{
    ISteering _steering;
    ObstacleAvoidance _avoidance;


    public EnemyStateChase(ISteering steering, ObstacleAvoidance avoidance)
    {
        _steering = steering;
        _avoidance = avoidance;
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
           

            var dir = (_steering.GetDir() + _avoidance.GetDir() * 2f);
            _model.Move(dir);
            _model.LookDir(dir);
        }
      
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;

    }
}
