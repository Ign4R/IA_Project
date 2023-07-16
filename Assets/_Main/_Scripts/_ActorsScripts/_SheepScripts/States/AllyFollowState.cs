using UnityEngine;

public class AllyFollowState<T> : NavigationState<T>
{
    FlockingManager _flockingManager;
    AllyModel _sheep;
    T _inputIdle;
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
        _sheep = (AllyModel)_model;
    }
    public override void Awake()
    {

        _model.OnRun += _view.AnimRun;
        base.Awake();
    }
    public override void Execute()
    {
        Debug.Log("Execute Follow state");
        base.Execute();
        bool finishGame = GameManager.Instance.FinishGame;
        Transform target = _flockingManager.HisLeader;
        var distance = Vector3.Distance(_model.transform.position, target.position);

        if(_sheep.IsStop || finishGame)
        {
            _fsm.Transitions(_inputIdle);
      
        }
        else
        {
            _sheep.Move(_sheep.Front);
            _sheep.Icon.material.color = Color.green;
            Vector3 flockingDir = _flockingManager.RunFlockingDir().normalized;
            if (distance <= 6)
            {
                Vector3 repel = _model.transform.position - target.position;
                Vector3 endDir = repel.normalized * 6;
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
    }


}

