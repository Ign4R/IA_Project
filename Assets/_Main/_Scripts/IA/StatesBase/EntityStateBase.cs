public class EntityStateBase<T> : State<T>
{
    protected BaseModel _model;
    protected BaseView  _view;
    protected FSM<T> _fsm;
    public virtual void InitializedState(BaseModel model, BaseView view,FSM<T> fsm)
    {
        _model = model;
        _fsm = fsm;
        _view = view;
    }
}
