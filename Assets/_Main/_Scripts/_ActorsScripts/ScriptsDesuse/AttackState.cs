using UnityEngine;

public class AttackState<T> : NavigationState<T>
{
    float _timerValue;
    NPCLeader_M _enemyM;

    public AttackState(float timerState) : base(null)
    {
        _timerValue = timerState;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyM = (NPCLeader_M)model;

    }
    public override void Awake()
    {
        base.Awake();
        _model.Move(Vector3.zero);
        CurrentTimer = _timerValue;
        _enemyM.AttackTimeActive = true;
    }

    public override void Execute()
    {

        base.Execute();  
        if (CurrentTimer > 0)
        {

            DecreaseTimer();
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
        Debug.Log("Sleep Atttack state");
        _enemyM.AttackTimeActive = false;
        _enemyM.OnAttack(false);
    }


}
