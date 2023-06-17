using System.Collections;
using UnityEngine;

public class NavigationState<T> : EntityStateBase<T>
{
    protected EnemyModel _enemyModel;
    public IWaypoint<Node> Wp { get; protected set; }

    public float CurrentTimer { get; protected set; }
    public float TimerValue { get; private set; }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        Wp = model as IWaypoint<Node>;
        _enemyModel = model as EnemyModel;

    }
    public virtual void SetTimer(float timer)
    {
      
    }
    protected int SetRandomTimer(float maxFloat)
    {
        int timer = Random.Range(1, (int)maxFloat);
        return timer;
    }

    public void RunTimer()
    {
        CurrentTimer -= Time.deltaTime;
    } 

  

}