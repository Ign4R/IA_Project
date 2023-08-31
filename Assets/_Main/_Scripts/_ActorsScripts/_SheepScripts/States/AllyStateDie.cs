public class AllyStateDie<T>:EntityStateBase<T>
{
    private AllyModel _sheepM;


    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)_model;

    }

    public override void Awake()
    {
        base.Awake();
      
    }

}