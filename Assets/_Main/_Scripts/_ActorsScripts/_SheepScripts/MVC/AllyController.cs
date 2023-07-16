using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStateEnum> _fsm;
    private FlockingManager _flockManager;
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
        _fsm = new FSM<AllyStateEnum>();

        var idle = new AllyIdleState<AllyStateEnum>();
        var walk = new AllyWalkState<AllyStateEnum>();
        var follow = new AllyFollowState<AllyStateEnum>(AllyStateEnum.Idle, _flockManager);
        var spawn = new AllySpawnState<AllyStateEnum>();


        idle.InitializedState(_model, _view, _fsm);
        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        ///Add Transitions
        ///idle
        idle.AddTransition(AllyStateEnum.Follow, follow);

        ///walk
        walk.AddTransition(AllyStateEnum.Follow, follow);
        walk.AddTransition(AllyStateEnum.Idle, idle);
        ///follow
        follow.AddTransition(AllyStateEnum.Idle, idle);
        follow.AddTransition(AllyStateEnum.Walk, walk);
        follow.AddTransition(AllyStateEnum.Procreating, spawn);
        ///procreate
        spawn.AddTransition(AllyStateEnum.Follow, follow);
 

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
        return _flockManager.HisLeader != null;
    }
    public void ActionIdle()
    {

    }

    public void ActionWalk()
    {
        _fsm.Transitions(AllyStateEnum.Walk);
    }
    public void ActionSpawn()
    {
        
    }
    public void ActionFollow()
    {
        _fsm.Transitions(AllyStateEnum.Follow);
    }
}