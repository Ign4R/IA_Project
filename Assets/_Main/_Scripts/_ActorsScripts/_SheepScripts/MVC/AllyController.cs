using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
   
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
        //var obsAvoid = new ObstacleAvoidance(_model.transform, _model._maskAvoid, _model._maxObs, _model._angleAvoid, _model._radiusAvoid);
        _fsm = new FSM<AllyStates>();

        var idle = new AllyIdleState<AllyStates>();
        var walk = new AllyWalkState<AllyStates>();
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flockManager);
        var spawn = new AllySpawnState<AllyStates>();


        idle.InitializedState(_model, _view, _fsm);
        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        ///Add Transitions
        ///idle
        idle.AddTransition(AllyStates.Follow, follow);

        ///walk
        walk.AddTransition(AllyStates.Follow, follow);
        walk.AddTransition(AllyStates.Idle, idle);
        ///follow
        follow.AddTransition(AllyStates.Idle, idle);
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Procreating, spawn);
        ///procreate
        spawn.AddTransition(AllyStates.Follow, follow);
 

        ///Initialize
        idle.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);

        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        var idle = new TreeAction(ActionIdle);
        var walk = new TreeAction(ActionWalk);
        var follow= new TreeAction(ActionFollow);
        var spawn = new TreeAction(ActionSpawn);


        ///tengo un leader?
        var hasLeader = new TreeQuestion(HasLeader, follow, walk);
        _root = hasLeader;
    }

    bool HasLeader()
    {
        return _model.TheirLeader != null;
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
    public void ActionFollow()
    {
        _fsm.Transitions(AllyStates.Follow);
    }
}