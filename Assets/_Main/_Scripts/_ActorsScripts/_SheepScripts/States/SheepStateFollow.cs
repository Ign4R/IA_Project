using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepStateFollow<T> : EntityStateBase<T>
{
    FlockingManager _flockingManager;
    SheepModel _sheep;
    T _inputIdle;
    bool _targetIsDead;

    public SheepStateFollow(T inputIdle, FlockingManager flockingManager)
    {
        _flockingManager = flockingManager;
        _inputIdle = inputIdle;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheep = (SheepModel)_model;
    }
    public override void Awake()
    {
        

        base.Awake();
    }
    public override void Execute()
    {
        Debug.Log("Follow State");
        base.Execute();
        bool targetDead = GameManager.Instance.PlayerIsDie;
        if (!_sheep.IsStop && !targetDead)
        {         
            Vector3 flockingDir = _flockingManager.RunFlockingDir().normalized;
            _model.LookDir(flockingDir);
            _sheep.Move(_sheep.Front);
        }
        else
        {
            _fsm.Transitions(_inputIdle);
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        //_model.Move(Vector3.zero);
        //_model.OnRun -= _view.AnimRun;
    }

}