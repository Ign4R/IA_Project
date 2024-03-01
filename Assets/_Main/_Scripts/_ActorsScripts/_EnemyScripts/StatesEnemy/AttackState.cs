using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : NavigationState<T>
{
    float _timerValue;
    SpiderModel _enemyM;

    public EnemyAttackState(float timerState, ISteering steering) : base(null)
    {
        _timerValue = timerState;
        _steering = steering;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyM = (SpiderModel)model;

    }
    public override void Awake()
    {
        base.Awake();
        Vector3 lookDir = _steering.GetDir().normalized;
        _model.Move(Vector3.zero);
        _model.LookDir(lookDir);
        CurrentTimer = _timerValue;
        _enemyM.AttackTimeActive = true;


    }

    public override void Execute()
    {

        base.Execute();
        if (CurrentTimer > 0)
        {

            Timer();
            _enemyM.CurrentTimerAttack = CurrentTimer;
            _model.Move(Vector3.zero);
            _model.LookDir(_model.transform.forward);

        }
        else
        {

            _enemyM.AttackTimeActive = false;

        }



    }
    public override void Sleep()
    {
        base.Sleep();

        _enemyM.AttackTimeActive = false;
        _enemyM.OnAttack(false);
    }


}