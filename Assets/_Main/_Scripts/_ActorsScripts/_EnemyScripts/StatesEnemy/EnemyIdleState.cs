using UnityEngine;

public class EnemyIdleState<T> : EntityStateBase<T>
{
    public override void Awake()
    {
        base.Awake();
        Debug.Log("Awake Idle state" + " Enemy");
        _model.OnRun += _view.RunAnim;
        _model.Move(Vector3.zero);
    }
    public override void Execute()
    {
        Debug.Log("Execute Idle state" + " Enemy");
        _fsm.Transitions(default);
    }
    public override void Sleep()
    {
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.RunAnim;
        Debug.Log("Sleep Idle state" + " Enemy");
        base.Sleep();
       
    }



}
