using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHurting<T> : EntityStateBase<T>
{
    PlayerModel playerModel;
    T _inputRunning;
    public PlayerStateHurting(T inputRunning)
    {
        _inputRunning = inputRunning;
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);


    }
    public override void Awake()
    {
    
        base.Awake();

        //NO HIT
    }
    public override void Execute()
    {
        Debug.Log("HURT PLAYER");

        var h = Input.GetAxis("Vertical");
        var dir = h * _model.transform.forward;
        base.Execute();
        _model.Move(dir);
        //TIMER TO TRANSITION
    }
    public override void Sleep()
    {
        base.Sleep();

      //YEP HIT
       
    }

}


