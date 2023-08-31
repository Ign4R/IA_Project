using UnityEngine;

public class AllyFollowState<T> : NavigationState<T>
{
    FlockingManager _flk;
    AllyModel _sheepM;
    AllyView _sheepV;
    T _inputIdle;
    Transform _target;
    float _maxAffinity;



    public AllyFollowState(T inputIdle, FlockingManager flockingManager, float maxFidelity,ISteering obsAvoid): base(obsAvoid)//TODO
    {
        _flk = flockingManager;
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
        _flk.GetFlockLeader(_target);
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
        Vector3 flockingDir = _flk.RunFlockingDir().normalized * _flk._multiplierFLK;
        if (distance <= 6)
        {
            Vector3 repel = _model.transform.position - _target.position;
            Vector3 endDir = repel.normalized * 2;
            _model.LookDir(endDir);
        }
        else
        {
     
            var avoid = Avoid.GetDir().normalized * _sheepM._multiplierAvoid;
            var endDir = flockingDir + avoid;
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
        _flk.Clear();
        _flk.GetFlockLeader(null);

    }


}

