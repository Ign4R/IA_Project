using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public Leadership _leadership;
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStates> _fsm;
    private FlockingManager _flockManager;
    [ReadOnly] public AllyStates _currentState;
    ITreeNode _root;

    private void Awake()
    {
        _model = GetComponent<AllyModel>();
        _view = GetComponent<AllyView>();
        _leadership = GetComponent<Leadership>();
        _flockManager = GetComponent<FlockingManager>();
        InitializedFSM();
        InitializeTree();
    }
    private void Start()
    {
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    
    public void InitializedFSM()
    {
        _fsm = new FSM<AllyStates>();

        var idle = new AllyIdleState<AllyStates>();
        var walk = new AllyWalkState<AllyStates>();
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flockManager);
        var spawn = new AllySpawnState<AllyStates>();
        var escape = new AllyEscapeState<AllyStates>();


        idle.InitializedState(_model, _view, _fsm);
        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        escape.InitializedState(_model, _view, _fsm);
        ///Add Transitions
        ///idle
        idle.AddTransition(AllyStates.Follow, follow);

        ///walk
        walk.AddTransition(AllyStates.Follow, follow);
        walk.AddTransition(AllyStates.Idle, idle);
        walk.AddTransition(AllyStates.Escape, idle);
        ///follow
        follow.AddTransition(AllyStates.Idle, idle);
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Procreating, spawn);
        follow.AddTransition(AllyStates.Escape, escape);
        ///procreate
        spawn.AddTransition(AllyStates.Follow, follow);
        ///escape
        escape.AddTransition(AllyStates.Idle, idle);
        escape.AddTransition(AllyStates.Walk, walk);
        escape.AddTransition(AllyStates.Follow, follow);


        ///Initialize
        idle.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        escape.InitializedState(_model, _view, _fsm);

        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        var idle = new TreeAction(ActionIdle);
        var walk = new TreeAction(ActionWalk);
        var follow= new TreeAction(ActionFollow);
        var spawn = new TreeAction(ActionSpawn);
        var escape = new TreeAction(ActionEscape);


        ///tengo un leader?
        var inRisk = new TreeQuestion(InRisk, escape, follow);
        var hasLeader = new TreeQuestion(HasLeader, inRisk, walk);
        _root = hasLeader;
    }
    bool InRisk()
    {
        return _model._leaders.Count > 1;
    }
    bool HasLeader()
    {
        return _model.HasLeader;

    }
    public void ActionIdle()
    {

    }

    public void ActionWalk()
    {
        _currentState = AllyStates.Walk;
        _fsm.Transitions(_currentState);
    }
    public void ActionSpawn()
    {
        
    }
    public void ActionEscape()
    {
        _currentState = AllyStates.Escape;
        _fsm.Transitions(_currentState);
    }
    public void ActionFollow()
    {
        _currentState = AllyStates.Follow;
        _fsm.Transitions(_currentState);
    }
}