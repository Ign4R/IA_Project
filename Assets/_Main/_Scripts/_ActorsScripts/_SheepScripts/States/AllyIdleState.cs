using System.Collections;
using UnityEngine;


public class AllyIdleState<T> : EntityStateBase<T>
{
  
    public override void Awake()
    {     
        base.Awake();

        _model.Move(Vector3.zero);
        _model.transform.forward = Vector3.forward;
        _model.LookDir(_model.GetForward);
        _view.IdleAnim(true);


    }
    public override void Execute()
    {
        base.Execute();
        _model.Move(Vector3.zero);
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.RunAnim;
    }
}
