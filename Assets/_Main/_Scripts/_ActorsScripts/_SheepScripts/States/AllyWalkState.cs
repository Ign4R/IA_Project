using UnityEditor;
using UnityEngine;

public class AllyWalkState<T> : EntityStateBase<T>
{
    private float randomDirectionInterval = 2f; // Intervalo de tiempo para cambiar a dirección RANDOM
    private float _randomDirectionTimer = 0;
    private float multiplier = 7;
    private int _stuckCounter = 0;
    private int _maxStuck = 10;
    private Transform _lastPos;
    private AllyModel _sheepM;
    private AllyView _sheepV;
    private Vector3 _randomDirection;

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _sheepV = (AllyView)view;
    }

    public override void Awake()
    {
        //TODO
        Debug.Log(" [Awake en Walk State] ");
        base.Awake();
        _sheepM.InRisk = false;
        _sheepM.HasLeader = false;
        _model.OnRun += _view.RunAnim;
        _sheepV.ChangeColor(Color.white);
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log(" [Execute de Walk State] ");
        if (_lastPos == _model.transform)
            CheckIfStuck();

        _randomDirectionTimer -= Time.deltaTime;
        if (_randomDirectionTimer <= 0) 
            GetRandomDirection();
     
        Vector3 randomDir = (_randomDirection * multiplier).normalized;
        _sheepM.Move(_sheepM.Front);
        _model.LookDir(randomDir);
    }

    void GetRandomDirection()
    {
        _stuckCounter = 0;
        _randomDirectionTimer = randomDirectionInterval;
        _randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void CheckIfStuck()
    {
        _stuckCounter++;
        if (_stuckCounter >= _maxStuck)
            GetRandomDirection();
    }

    public override void Sleep()
    {
        Debug.Log(" [Sleep de Walk State] ");
        base.Sleep();
        _model.OnRun -= _view.RunAnim;
    }
}
