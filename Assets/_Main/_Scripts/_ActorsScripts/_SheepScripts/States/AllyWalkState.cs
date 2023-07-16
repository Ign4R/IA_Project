using UnityEditor;
using UnityEngine;

public class AllyWalkState<T> : EntityStateBase<T>
{
    AllyModel _sheep;
    public float randomDirectionInterval = 2f; // Intervalo de tiempo para cambiar a dirección RANDOM
    public float multiplier = 7;
    private Vector3 _randomDirection;
    private float _randomDirectionTimer;
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheep = (AllyModel)model;
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Execute Walk State");
        _sheep.Icon.material.color = Color.white;
        _sheep.Move(_sheep.Front);
        _randomDirectionTimer -= Time.deltaTime;
        if (_randomDirectionTimer <= 0)
        {
            _randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _randomDirectionTimer = randomDirectionInterval;
        }
        Vector3 randomDir = _randomDirection * multiplier;
        _model.LookDir(randomDir.normalized);
    }
    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
    }
}
