using UnityEngine;

public class EnemyIdleState<T> : EntityStateBase<T>
{
    public override void Awake()
    {
        base.Awake();

        _model.OnRun += _view.AnimRun;
        _model.Move(Vector3.zero);
    }
    public override void Execute()
    {

        _fsm.Transitions(default);
    }
    public override void Sleep()
    {
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;

        base.Sleep();
       
    }



}
