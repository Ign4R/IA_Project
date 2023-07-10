using UnityEngine;

public class EnemyIdleState<T> : EntityStateBase<T>
{

    public override void Awake()
    {
        base.Awake();
        Debug.Log("Idle Awake");
        _model.OnRun += _view.AnimRun;
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;
    }
    
}
