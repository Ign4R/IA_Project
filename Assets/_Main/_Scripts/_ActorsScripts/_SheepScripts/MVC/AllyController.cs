using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float _againP,_walkP,_deadP;
    public float _timerScare;
    public Leadership _leadership;
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStates> _fsm;
    private FlockingManager _flockManager;
    private TreeAction _actionRandom;
    [ReadOnly] [SerializeField] private AllyStates _currentState;
    ITreeNode _root;
    Dictionary<AllyStates, float> _randomWS = new Dictionary<AllyStates, float>();

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

    
    public void InitializedFSM()
    {
        _fsm = new FSM<AllyStates>();

        var walk = new AllyWalkState<AllyStates>();
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flockManager);
        var still = new AllyStillState<AllyStates>(_timerScare,AllyStates.Default);
        var again = new AgainRepeatState<AllyStates>();

        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        still.InitializedState(_model, _view, _fsm);
        again.InitializedState(_model, _view, _fsm);
        ///Add Transitions

        ///walk
        walk.AddTransition(AllyStates.Follow, follow);

        ///follow
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Still, still);

        ///escape
        still.AddTransition(AllyStates.Walk, walk);
        still.AddTransition(AllyStates.Follow, follow);
        still.AddTransition(AllyStates.AgainState, again);

        ///again
        again.AddTransition(AllyStates.Still, still);




        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        TreeAction walk = new TreeAction(ActionWalk,AllyStates.Walk);
        TreeAction follow = new TreeAction(ActionFollow,AllyStates.Follow);
        TreeAction still = new TreeAction(ActionStill,AllyStates.Still);
        TreeAction dead = new TreeAction(default);
        TreeAction again = new TreeAction(ActionAgain,AllyStates.AgainState);
        TreeAction randomWS = new TreeAction(ActionRandomWS);

        _randomWS.Add(AllyStates.AgainState, _againP);
        _randomWS.Add(AllyStates.Walk, _walkP);
        _randomWS.Add(AllyStates.Default, _deadP);

        TreeQuestion isScareOver = new TreeQuestion(IsScareOver, randomWS, still);
        ///hay dos lideres?
        TreeQuestion hasTwoLeader = new TreeQuestion(HasTwoLeader, isScareOver, follow);
        TreeQuestion hasLeader = new TreeQuestion(HasLeader, hasTwoLeader, walk);
        _root = hasLeader;
    }
    bool IsScareOver()
    {
        return _model._scareCurrTimer < 1 && !_fsm.StateRepeat;
    }
    bool HasTwoLeader()
    {
        List<NPCLeader_M> leads= _model._leaders;
        return leads.Count > 1 || _model.InRisk;
    }
    bool HasLeader()
    {
        return _model.HasLeader;
    }
    public void ActionRandomWS()
    {
        _randomWS[AllyStates.AgainState] = _againP;
        _randomWS[AllyStates.Walk] = _walkP;
        _randomWS[AllyStates.Die] = _deadP;

        _currentState = MyRandoms.Roulette(_randomWS);
        Debug.LogWarning(_currentState);
        _fsm.Transitions(_currentState);

    }
    public void ActionWalk()
    {
        _currentState = AllyStates.Walk;
        _fsm.Transitions(_currentState);
    }
    public void ActionAgain()
    {
        _currentState = AllyStates.AgainState;
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
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();     
    }

}