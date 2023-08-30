using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AllyStayState<T> : EntityStateBase<T>
{
    AllyModel _sheepM;
    AllyView _sheepV;
    float _timerValue;
    FlockingManager _flkM;
    T _input;
    Collider[] _colliders;

    public AllyStayState(float timer, T input, FlockingManager flkM)
    {
        _timerValue = timer;
        _input = input;
        _flkM = flkM;
   
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        CurrentTimer = _timerValue;
        _sheepM = (AllyModel)model;
        _sheepV = (AllyView)view;
        _colliders = new Collider[_flkM.maxBoids];
    }

    public override void Awake()
    {
        base.Awake();

        _sheepM._fidelity--;
        _sheepM._leaders.Clear();
        CurrentTimer = _timerValue;
        _sheepV.ChangeColor(Color.white);
        _view.IdleAnim(true);
        _sheepM.InRisk = true;
        //_sheepM._alliesNear = Physics.OverlapSphereNonAlloc(_sheepM.Position, _sheepM.Radius, _colliders, _flkM.maskBoids);
        Debug.Log("Entre en Stay State");
 
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
            ModifyTimer();
            _sheepM.Move(Vector3.zero);
        }
 
    }

    public override void Sleep()
    {
        Debug.Log("Sali en Stay State");
        base.Sleep();   
        _view.IdleAnim(false);
       
      
    }
}

