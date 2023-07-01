using System.Collections;
using UnityEngine;


public class SheepStateIdle<T> : EntityStateBase<T>
{

   
    public override void Awake()
    {
      
        base.Awake();
        _model.transform.forward = Vector3.forward;
        _model.Move(Vector3.zero);
      
    }
    public override void Sleep()
    {
        base.Sleep();

    }




}
