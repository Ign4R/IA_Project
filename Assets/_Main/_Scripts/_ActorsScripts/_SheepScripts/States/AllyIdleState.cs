using System.Collections;
using UnityEngine;


public class AllyIdleState<T> : EntityStateBase<T>
{
  
    public override void Awake()
    {     
        base.Awake();

        _model.OnRun += _view.RunAnim;
        _model.transform.forward = Vector3.forward;
        _model.Move(Vector3.zero);
        _model.LookDir(_model.GetForward);


    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.RunAnim;
    }
}
