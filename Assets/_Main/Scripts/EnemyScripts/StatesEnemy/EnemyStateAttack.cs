using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : NavigationState<T>
{
    float _timer;
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

        base.Execute();  
        if (CurrentTimer > 0)
        {
            Debug.Log("Attack Duration Active");
            RunTimer();
            _enemyModel.CurrentTimerAttack = CurrentTimer;
            _model.Move(Vector3.zero);
            _model.LookDir(_model.transform.forward);
            _enemyModel.AttackTimeActive = true;
        }
        else
        {
            Debug.Log("Attack Duration Desactive"); ///TODO
            _enemyModel.AttackTimeActive = false;

        }



    }
    public override void Sleep()
    {
        base.Sleep();
        _model.OnAttack(false);
    }


}
