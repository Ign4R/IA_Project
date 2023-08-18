using UnityEngine;
public class StealState<T>: EntityStateBase<T>
{
    NPCLeader_M _leaderM;
    Collider[] _colls;
    LayerMask _allyMask;
    AllyModel ally;
    float _timer;
    T _input;

    public StealState(float timer ,LayerMask allies)
    {

        _timer = timer;
        _allyMask = allies;
        _colls = new Collider[5];
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _leaderM = (NPCLeader_M)model;
    }

    public override void Awake()
    {
        CurrentTimer = _timer;
        _model.OnRun += _view.AnimRun;
        base.Awake();
        _model.Move(Vector3.zero);
        base.Execute();
        int count = Physics.OverlapSphereNonAlloc(_model.transform.position, _leaderM._radiusView, _colls, _allyMask);

        for (int i = 0; i < count; i++)
        {
            var curr = _colls[0];
            ally = curr.GetComponent<AllyModel>();
        }
    }
    public override void Execute()
    {
        Debug.Log("Steel Execute State");
        base.Execute();
       
        if (CurrentTimer > 0)
        {
            DecreaseTimer();
        }
        else
        {
            ally.IsStealable = true;
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
    }


}