using UnityEngine;

public class PlayerIdleState<T> : EntityStateBase<T>
{
    T _inputRunning;
    public PlayerIdleState(T inputRunning)
    {
        _inputRunning = inputRunning;
    }
    public override void Execute()
    {
      
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        _model.LookDir(Vector3.up);
        if (h != 0 || v != 0 )
        {
            _fsm.Transitions(_inputRunning);
        }
    }
}
