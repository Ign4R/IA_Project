using UnityEngine;

public class AllyFollowState<T> : NavigationState<T>
{
    FlockingManager _flk;
    AllyModel _sheepM;
    AllyView _sheepV;
    T _inputIdle;
    Transform _target;
    float _maxAffinity;
    float _maxDistance;



    public AllyFollowState(T inputIdle, FlockingManager flockingManager, float maxFidelity,ISteering obsAvoid, float maxDistance): base(obsAvoid)//TODO
    {
        _flk = flockingManager;
        _inputIdle = inputIdle;
        _maxAffinity = maxFidelity;
        _maxDistance= maxDistance;
        _steering = obsAvoid;
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
        GameManager.Instance.CounterSheep();
        _target = _sheepM._leaders[0].transform;
        _flk.SetFlockLeader(_target);
        _sheepV.ChangeColor(_sheepM.ColorFollow);
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
                Timer(1f);
                IncreaseAffinity();
            }
       
        }

    }
    public void Flocking()
    {
        
        var distance = Vector3.Distance(_model.transform.position, _target.position);
        Vector3 flockingDir = _flk.RunFlockingDir().normalized; 
        var avoid = Avoid.GetDir().normalized * _sheepM._multiplierAvoid;
        var endDir = flockingDir;
        if (distance <= _maxDistance)
        {
            _sheepV.AnimIdle(true);
            _sheepM.Move(Vector3.zero);
            _sheepM.LookDir(endDir);
        }
        else
        {
            _sheepM.Move(_sheepM.Front);
            _model.LookDir(endDir);

        }

       
    }
    public void IncreaseAffinity()
    {    
        _sheepM._affinityW = (int)CurrentTimer * _sheepM.MultiplyAffinity;
    }
    public override void Sleep()
    {
        base.Sleep();
         _model.OnRun += _view.RunAnim;
        _flk.Clear();
        _flk.ResetFlockLeader();
        GameManager.Instance.ResetCountSheep();

    }


}

