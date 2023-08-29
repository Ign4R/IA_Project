using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AllyStillState<T> : EntityStateBase<T>
{
    AllyModel _sheepM;
    AllyView _sheepV;
    float _timerValue;
    T _input;

    public AllyStillState(float timer, T input)
    {
        _timerValue = timer;
        _input = input; 
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _sheepV = (AllyView)view;
    }

    public override void Awake()
    {
        base.Awake();
        _sheepM._leaders.Clear();
        CurrentTimer = _timerValue;
        _sheepV.ChangeColor(Color.white);
        _view.IdleAnim(true);
        _sheepM.InRisk = true;

        Debug.Log("Entre en Still State");
 
    }

    public override void Execute()
    {
        base.Execute();
        //if (_sheepM._leaders.Count > 0)
        //{
        //    if (_sheepM._leaders[0].leadColor != _sheepM.ColorTeam) return;
        //    _sheepM.InRisk = false;
        //}
        if (CurrentTimer > 0)
        {          
            _sheepM._scareCurrTimer = CurrentTimer;
            DecreaseTimer();
            _sheepM.Move(Vector3.zero);
        }
 
    }

    public override void Sleep()
    {
        Debug.Log("Sali en Still State");
        base.Sleep();   
        _view.IdleAnim(false);
       
      
    }
}

