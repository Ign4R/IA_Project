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



    public AllyFollowState(T inputIdle, FlockingManager flockingManager, float maxFidelity,ISteering obsAvoid, int maxDistance): base(obsAvoid)//TODO
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
        _target = _sheepM._leaders[0].transform;
        _flk.GetFlockLeader(_target);
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
        _sheepM.Move(_sheepM.Front);
        var distance = Vector3.Distance(_model.transform.position, _target.position);
        Vector3 flockingDir = (_flk.RunFlockingDir().normalized * _flk._multiplierFLK) + Avoid.GetDir();
        if (distance <= _maxDistance)
        {
            Vector3 repel = _model.transform.position - _target.position;
            Vector3 endDir = repel.normalized * 2;
            _sheepM.LookDir(endDir);
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
        _sheepM._affinityW = (int)CurrentTimer * _sheepM.MultiplyAffinity;
    }
    public override void Sleep()
    {
        base.Sleep();
        _flk.Clear();
        _flk.GetFlockLeader(null);

    }


}

