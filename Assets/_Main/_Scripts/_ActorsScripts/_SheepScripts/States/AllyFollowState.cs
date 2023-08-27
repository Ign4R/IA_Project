using UnityEngine;

public class AllyFollowState<T> : NavigationState<T>
{
    FlockingManager _flockingManager;
    AllyModel _sheepM;
    AllyView _sheepV;
    T _inputIdle;
    Transform _target;
    //public float _limitDistance = 23f; /// Distancia limite de seguimiento PLAYER


    public AllyFollowState(T inputIdle, FlockingManager flockingManager, ISteering obsAvoid=null): base(obsAvoid)//TODO
    {
        _flockingManager = flockingManager;
        _inputIdle = inputIdle;
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

        var distance = Vector3.Distance(_model.transform.position,_target.position);

        if(_sheepM.IsStop || finishGame)
        {
            _fsm.Transitions(_inputIdle);
      
        }
        else
        {
            _sheepM.Move(_sheepM.Front);
            _sheepM.Icon.material.color = Color.green;
            Vector3 flockingDir = _flockingManager.RunFlockingDir().normalized;
            if (distance <= 6)
            {
                Vector3 repel = _model.transform.position - _target.position;
                Vector3 endDir = repel.normalized*2;
                _model.LookDir(endDir);
            }
            else
            {
                var endDir = flockingDir;/* + Avoid.GetDir().normalized;*/ //TODO
                _model.LookDir(endDir);
            }


        }

    }

    public override void Sleep()
    {
        base.Sleep();
        _flockingManager.GetFlockLeader(null);

    }


}

