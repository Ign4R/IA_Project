using UnityEngine;

public class EntityStateBase<T> : State<T>
{
    protected BaseModel _model;
    protected BaseView  _view;
    protected FSM<T> _fsm;
    public float CurrentTimer { get; protected set; }
    public virtual void InitializedState(BaseModel model, BaseView view,FSM<T> fsm)
    {
        _model = model;
        _fsm = fsm;
        _view = view;
    }

    public virtual void SetTimer(float timer)
    {

    }
    protected int SetRandomTimer(float maxFloat)
    {
        int timer = Random.Range(1, (int)maxFloat);
        return timer;
    }

    public void DecreaseTimer()
    {
        CurrentTimer -= Time.deltaTime;

    }
}
