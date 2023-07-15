using System.Collections;
using UnityEngine;


public class SheepIdleState<T> : EntityStateBase<T>
{ 
    public override void Awake()
    {     
        base.Awake();
        Debug.Log("Awake Idle state" + " Sheep");
        _model.OnRun += _view.AnimRun;
        _model.transform.forward = Vector3.forward;
        _model.Move(Vector3.zero);
        _model.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        _model.OnRun -= _view.AnimRun;

    }


}
