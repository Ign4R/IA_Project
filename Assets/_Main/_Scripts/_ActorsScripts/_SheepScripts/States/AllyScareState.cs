using UnityEngine;
using UnityEngine.UIElements;

public class AllyScareState<T> : EntityStateBase<T>
{
    AllyModel _sheepM;
    AllyView _sheepV;
    float _timerValue;
    T _input;

    public AllyScareState(float timer, T input)
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
        _sheepV.ChangeColor(Color.white);
        _model.OnRun += _view.AnimRun;
        _model.gameObject.layer = default;
    }


    public override void Execute()
    {
        base.Execute();
        if (CurrentTimer > 0)
        {
            DecreaseTimer();
            _sheepM.Move(Vector3.zero);
            _model.LookDir(-_sheepM.Front);

        }
        else
        {
            _fsm.Transitions(_input);
        }


    }
    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
        _model.gameObject.layer = 13;
    }
}

