using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public SheepModel _model;
    public SheepView _view;
    FSM<BaseStatesEnum> _fsm;

    private void Awake()
    {
        _model.OnRun += _view.AnimRun;
        InitializedFSM();
    }
    private void Start()
    {
    }
    private void Update()
    {
        _fsm.OnUpdate();
    }
    public void InitializedFSM()
    {
        _fsm = new FSM<BaseStatesEnum>();
        var flockManager = GetComponent<FlockingManager>();
        var idle = new SheepStateIdle<BaseStatesEnum>();
        var move = new SheepStateFollow<BaseStatesEnum>(BaseStatesEnum.Idle, flockManager);

        idle.AddTransition(BaseStatesEnum.Movement, move);
        move.AddTransition(BaseStatesEnum.Idle, idle);
        idle.InitializedState(_model, _view, _fsm);
        move.InitializedState(_model, _view, _fsm);

        _fsm.SetInit(move);
    }

}