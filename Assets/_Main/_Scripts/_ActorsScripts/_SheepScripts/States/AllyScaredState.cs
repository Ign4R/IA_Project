using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AllyScaredState<T> : EntityStateBase<T>
{
    AllyModel _sheepM;
    AllyView _sheepV;
    float _timerValue;
    FlockingManager _flkM;
    T _input;
    Collider[] _colliders;

    public AllyScaredState(float timer, T input, FlockingManager flkM)
    {
        _timerValue = timer;
        _input = input;
        _flkM = flkM;
   
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _sheepV = (AllyView)view;
        _colliders = new Collider[_flkM._maxBoids];
        CurrentTimer = _timerValue;
    }

    public override void Awake()
    {
        CurrentTimer = _timerValue;
        base.Awake();
        _sheepV.ChangeColor(Color.yellow);
        _view.IdleAnim(true);
        _sheepM._leaders.Clear();
        SetInitScaredStats();
        _sheepM._alliesNear = Physics.OverlapSphereNonAlloc(_sheepM.Position, _sheepM.Radius, _colliders, _sheepM.allies);
        _sheepM._dieW = (int)(1 - Mathf.Clamp01(_sheepM._alliesNear - 1)) * _sheepM.MultiplyDie;
    }

    public void SetInitScaredStats()
    {    
        float increase = _sheepM.MultiplyLeave;
        _sheepM._leaveW += increase;
        _sheepM._affinityW--;
    }

    public override void Execute()
    {
        base.Execute();
 
        if (CurrentTimer > 0)
        {          
            Timer();
            _sheepM.Move(Vector3.zero);
        }
 
    }

    public override void Sleep()
    {
        base.Sleep();
        _flkM.ResetFlockLeader();
        _view.IdleAnim(false);

    }
}

