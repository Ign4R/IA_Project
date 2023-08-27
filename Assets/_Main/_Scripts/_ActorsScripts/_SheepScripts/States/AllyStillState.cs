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
        CurrentTimer = _timerValue;
        _sheepM._leaders.Clear();
        _sheepM.InRisk = true;
        _sheepV.ChangeColor(Color.white);
        _view.IdleAnim(true);
 
    }

    public override void Execute()
    {
        base.Execute();
        if (_sheepM._leaders.Count > 0)
        {
            if (_sheepM._leaders[0].leadColor != _sheepM.ColorTeam) return;
            _sheepM.InRisk = false;
        }
        if (CurrentTimer > 0)
        {
            DecreaseTimer();
            _sheepM.Move(Vector3.zero);
        }
        else 
        {
            
            //TODO: Transiciona a estado de Dead? o al estado de Walk?
            //Usar Random Wheel Selection
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        _sheepM.HasLeader = false;
        _sheepM.InRisk = false;
        _view.IdleAnim(false);
      
    }
}

