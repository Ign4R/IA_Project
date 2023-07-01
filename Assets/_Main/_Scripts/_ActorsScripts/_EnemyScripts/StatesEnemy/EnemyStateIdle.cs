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
        _model.Move(Vector3.zero);
    }
}
