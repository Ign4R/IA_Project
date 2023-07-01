using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : NavigationState<T>
{
    float _timer;
    EnemyModel _enemyM;
    EnemyView _enemyV;

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyM = (EnemyModel)model;
        _enemyV = (EnemyView)view;
    }
    public override void Awake()
    {
        base.Awake();
        _model.Move(Vector3.zero);
        CurrentTimer = _timer;     
    }

    public override void SetTimer(float timer)
    {
        _timer = timer;
    }

    public override void Execute()
    {
        Debug.Log("Execute attack state ");
        base.Execute();  
        if (CurrentTimer > 0)
        {
            Debug.Log("Attack Duration Active");
            RunTimer();
            _enemyM.CurrentTimerAttack = CurrentTimer;
            _model.Move(Vector3.zero);
            _model.LookDir(_model.transform.forward);
            _enemyM.AttackTimeActive = true;
        }
        else
        {
            Debug.Log("Attack Duration Desactive"); ///TODO
            _enemyM.AttackTimeActive = false;

        }



    }
    public override void Sleep()
    {
        Debug.Log("Sleep atttack state");
        _enemyM.OnAttack(false);
        base.Sleep();
    }


}
