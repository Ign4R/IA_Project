using UnityEngine;

public class AllyFollowState<T> : NavigationState<T>
{
    FlockingManager _flockingManager;
    AllyModel _sheepM;
    AllyView _sheepV;
    T _inputIdle;
    Transform _target;
    float _maxAffinity;

    //public float _limitDistance = 23f; /// Distancia limite de seguimiento PLAYER


    public AllyFollowState(T inputIdle, FlockingManager flockingManager, float maxFidelity,ISteering obsAvoid=null): base(obsAvoid)//TODO
    {
        _flockingManager = flockingManager;
        _inputIdle = inputIdle;
        _maxAffinity = maxFidelity;
        //_steering = steering;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)_model;
        _sheepV = (AllyView)_view;
    }
    public override void Awake()
    {
        base.Awake();
        _target = _sheepM._leaders[0].transform;
        _flockingManager.GetFlockLeader(_target);
        _sheepV.ChangeColor(_sheepM.ColorTeam);
        _model.OnRun += _view.RunAnim;
    }
    public override void Execute()
    {
      
        base.Execute();
        bool finishGame = GameManager.Instance.FinishGame;

       

        if(_sheepM.IsStop || finishGame)
        {
            _fsm.Transitions(_inputIdle);
      
        }
        else
        {         
            Flocking();
            if (_sheepM._affinityW < _maxAffinity)
            {
                IncreaseAffinity();
            }
       
        }

    }
    public void Flocking()
    {
        _sheepM.Move(_sheepM.Front);
        var distance = Vector3.Distance(_model.transform.position, _target.position);
        Vector3 flockingDir = _flockingManager.RunFlockingDir().normalized;
        if (distance <= 6)
        {
            Vector3 repel = _model.transform.position - _target.position;
            Vector3 endDir = repel.normalized * 2;
            _model.LookDir(endDir);
        }
        else
        {
            var endDir = flockingDir;/* + Avoid.GetDir().normalized;*/ //TODO
            _model.LookDir(endDir);
        }
    }
    public void IncreaseAffinity()
    {
        int multiplierIncrease = 10;
        ModifyTimer(1f);
        _sheepM._affinityW = (int)CurrentTimer * multiplierIncrease;
    }
    public override void Sleep()
    {
        base.Sleep();
        _flockingManager.Clear();
        _flockingManager.GetFlockLeader(null);

    }


}

