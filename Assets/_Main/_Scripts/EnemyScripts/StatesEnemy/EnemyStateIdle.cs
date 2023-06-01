using UnityEngine;

public class EnemyStateIdle<T> : NavigationState<T>
{
    public T _patrolling;
    float _timer;
  
    public EnemyStateIdle(T inputPatrolling)
    {
        _patrolling = inputPatrolling;
    }
    public override void Awake()
    {
        base.Awake();
        CurrentTimer = SetRandomTimer(_timer);

        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;
    }
    public override void Execute()
    {   
        base.Execute();
       
        if (CurrentTimer > 0)
        {
            _enemyModel.CurrentTimerIdle = CurrentTimer;
            RunTimer();
        }
        else
        {
            _fsm.Transitions(_patrolling);
        }
           
    }

    public override void SetTimer(float timer)
    {
        _timer = timer;
    }

    public override void Sleep()
    {
        base.Sleep();
        CurrentTimer = 0f;
        Wp.IterationsInWp = 0;
    }
}
