using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : NavigationState<T>
{
    float _timerValue;
    EnemyModel _enemyM;
    ISteering _steering;

    public EnemyStateAttack(float timerState, ISteering steering)
    {
        _timerValue = timerState;
        _steering = steering;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyM = (EnemyModel)model;

    }
    public override void Awake()
    {
        base.Awake();
        Vector3 lookDir= _steering.GetDir().normalized;
        _model.Move(Vector3.zero);
        _model.LookDir(lookDir);
        CurrentTimer = _timerValue;
        _enemyM.AttackTimeActive = true;

     
    }

    public override void Execute()
    {
        Debug.Log("Execute attack state ");
        base.Execute();  
        if (CurrentTimer > 0)
        {
            Debug.Log("Attack Active");
            DecreaseTimer();
            _enemyM.CurrentTimerAttack = CurrentTimer;
            _model.Move(Vector3.zero);
            _model.LookDir(_model.transform.forward);

        }
        else
        {
            Debug.Log("Attack Desactive"); ///TODO
            _enemyM.AttackTimeActive = false;

        }



    }
    public override void Sleep()
    {
        Debug.Log("Sleep atttack state");
        _enemyM.AttackTimeActive = false;
        _enemyM.OnAttack(false);
        base.Sleep();
    }


}
