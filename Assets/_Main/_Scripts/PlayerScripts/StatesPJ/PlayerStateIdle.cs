using UnityEngine;

public class PlayerStateIdle<T> : EntityStateBase<T>
{
    T _inputRunning;
    public PlayerStateIdle(T inputRunning)
    {
        _inputRunning = inputRunning;
    }
    public override void Awake()
    {
        base.Awake();
   
    }
    public override void Execute()
    {
        Debug.Log("State Player idle");
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        if (!_model.IsDie)
        {
            _model.LookDir(Vector3.up);

            if (h != 0 || v != 0)
            {
                _fsm.Transitions(_inputRunning);
            }
        }
        else
        {
            _model.Move(Vector3.zero);
        }

       
        
    }
}
