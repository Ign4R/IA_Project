using UnityEngine;

public class EnemyStateIdle<T> : NavigationState<T>
{
    public T _patrolling;
    public EnemyStateIdle(T inputPatrolling)
    {
        _patrolling = inputPatrolling;
    }
    public override void Awake()
    {
        base.Awake();
        Debug.Log("Idle Awake");
        _model.OnRun += _view.AnimRun;
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;
    }
    
}
