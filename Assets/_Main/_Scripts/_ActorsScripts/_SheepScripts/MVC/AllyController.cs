using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float _timerScare;
    public Leadership _leadership;
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStates> _fsm;
    private FlockingManager _flockManager;
    [ReadOnly] [SerializeField] private AllyStates _currentState;
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

        var walk = new AllyWalkState<AllyStates>();
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flockManager);
        var spawn = new AllySpawnState<AllyStates>();
        var still = new AllyStillState<AllyStates>(_timerScare,AllyStates.Walk);


        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        still.InitializedState(_model, _view, _fsm);
        ///Add Transitions

        ///walk
        walk.AddTransition(AllyStates.Follow, follow);

        ///follow
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Procreating, spawn);
        follow.AddTransition(AllyStates.Still, still);
        ///procreate
        spawn.AddTransition(AllyStates.Follow, follow);
        ///escape
        still.AddTransition(AllyStates.Walk, walk);
        still.AddTransition(AllyStates.Follow, follow);


        ///Initialize
        follow.InitializedState(_model, _view, _fsm);
        spawn.InitializedState(_model, _view, _fsm);
        still.InitializedState(_model, _view, _fsm);

        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        var walk = new TreeAction(ActionWalk);
        var follow= new TreeAction(ActionFollow);
        var still = new TreeAction(ActionStill);


        ///tengo un leader?
        var hasTwoLeader = new TreeQuestion(HasTwoLeader, still, follow);
        var hasLeader = new TreeQuestion(HasLeader, hasTwoLeader, walk);
        _root = hasLeader;
    }
    bool HasTwoLeader()
    {
        return _model._leaders.Count > 1 || _model.InRisk;
    }
    bool HasLeader()
    {
        return _model.HasLeader;
    }
    public void ActionWalk()
    {
        _currentState = AllyStates.Walk;
        _fsm.Transitions(_currentState);
    }
    public void ActionStill()
    {
        _currentState = AllyStates.Still;
        _fsm.Transitions(_currentState);
    }
    public void ActionFollow()
    {
        _currentState = AllyStates.Follow;
        _fsm.Transitions(_currentState);
    }
}