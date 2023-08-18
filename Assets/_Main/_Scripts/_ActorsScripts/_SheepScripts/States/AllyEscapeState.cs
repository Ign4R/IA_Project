using UnityEngine;

public class AllyEscapeState<T> : EntityStateBase<T>
{
    AllyModel _sheepM;
    AllyView _sheepV;

    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun;
        _model.LookDir(-_sheepM.Front*8);
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _sheepV = (AllyView)view;

    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Execute Escape State");
        _sheepV.ChangeColor(Color.white);
        _sheepM.Move(_sheepM.Front);
        if (Physics.Raycast(_sheepM.transform.position, _sheepM.Front, 12f, LayerMask.GetMask("WallsPath")))
        {
            // Cambiar de dirección al detectar un obstáculo
            Debug.Log("ENTRE EN RAYCAST");
            _model.LookDir(-_sheepM.Front * 8);
        }

    }
    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
    }
}

