using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<SheepStateEnum> _fsm;

    private void Awake()
    {
     
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
        //var obsAvoid = new ObstacleAvoidance(_model.transform, _model._maskAvoid, _model._maxObs, _model._angleAvoid, _model._radiusAvoid);
        _fsm = new FSM<SheepStateEnum>();
        FlockingManager flockManager = GetComponent<FlockingManager>();
        var idle = new SheepIdleState<SheepStateEnum>();
        var move = new SheepFollowState<SheepStateEnum>(SheepStateEnum.Idle, flockManager, _maxDistance);
        var procreate = new SheepProcreationState<SheepStateEnum>();

        ///Add Transitions
        idle.AddTransition(SheepStateEnum.Movement, move);
        move.AddTransition(SheepStateEnum.Idle, idle);
        move.AddTransition(SheepStateEnum.Procreating, procreate);
        procreate.AddTransition(SheepStateEnum.Movement, move);
        ///Initialize
        idle.InitializedState(_model, _view, _fsm);
        move.InitializedState(_model, _view, _fsm);
        procreate.InitializedState(_model, _view, _fsm);

        _fsm.SetInit(move);
    }



}