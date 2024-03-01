using UnityEngine;
public class ChaseState<T> : NavigationState<T> 
{
    SpiderModel _spiderModel;
    Light _coneVision;
    public ChaseState(ISteering steering, ISteering obsAvoid):base(obsAvoid)
    {
        _steering = steering;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _spiderModel = (SpiderModel)model;
        _coneVision = _spiderModel._coneOfView;
    }

    public override void Awake()
    {      
        base.Awake();
        _model.OnRun += _view.RunAnim;
        _spiderModel.SpottedTarget = true;
        _coneVision.enabled = true;
        _coneVision.color = Color.red;
    }

    public override void Execute()
    {    
        base.Execute();

        Vector3 dirTarget = _steering.GetDir() * _spiderModel._multiplierPursuit;
        Vector3 dirAvoid = Avoid.GetDir() * _spiderModel._multiplierAvoid;
        Vector3 dirFinal = dirTarget + dirAvoid;

        _model.Move(dirFinal.normalized);
        _model.LookDir(dirFinal.normalized);
    }

    public override void Sleep()
    {
        base.Sleep();
        _coneVision.enabled = false;
        _model.OnRun -= _view.RunAnim;
    }
}
