using UnityEngine;
public class ChaseState<T> : NavigationState<T> 
{
    NPCLeader_M _leadModel;
    Light _coneVision;
    public ChaseState(ISteering steering, ISteering obsAvoid):base(obsAvoid)
    {
        _steering = steering;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _leadModel = (NPCLeader_M)model;
    }

    public override void Awake()
    {      
        base.Awake();
        _model.OnRun += _view.RunAnim;

        _coneVision = _leadModel._coneOfView;
        _coneVision.enabled = true;
        _coneVision.color = _leadModel.leadColor;
    }

    public override void Execute()
    {    
        base.Execute();

        Vector3 dirTarget = _steering.GetDir() * _leadModel._multiplierPursuit;
        Vector3 dirAvoid = Avoid.GetDir() * _leadModel._multiplierAvoid;
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
