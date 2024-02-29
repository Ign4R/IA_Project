using UnityEngine;

public class AllyStateDie<T>:EntityStateBase<T>
{
    private AllyModel _sheepM;
    private AllyView _sheepV;


    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _sheepV= (AllyView)view;

    }

    public override void Awake()
    {
        base.Awake();
        _sheepV.ChangeColor(Color.gray);
        _sheepM.Die();
        _sheepM.Freeze();
    }

}