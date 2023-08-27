using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState<T> : EntityStateBase<T>
{
    T _inputIdle;

    public PlayerMoveState(T inputIdle)
    {
        _inputIdle = inputIdle;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.RunAnim;
       
    }
    public override void Execute()
    {
        base.Execute();
        var h = Input.GetAxis("Vertical");
        var dir = h * _model.transform.forward;

        if (h == 0 || GameManager.Instance.FinishGame)
        {
            _fsm.Transitions(_inputIdle);
            return;
        }
        else
        {
            _model.Move(dir);
            _model.LookDir(Vector3.up);

        }
    }
    public override void Sleep()
    {
        base.Sleep();
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.RunAnim;
    }
}
