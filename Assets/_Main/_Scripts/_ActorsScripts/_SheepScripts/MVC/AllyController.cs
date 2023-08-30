using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public int _maxFidelity=10;
    public float _walkP, _followP;
    [ReadOnly][SerializeField] private float _stayTimer;
    [ReadOnly] [SerializeField] private AllyStates _currentState;
    public float _maxTimeScare;
    public Leadership _leadership;
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStates> _fsm;
    private FlockingManager _flockManager;
    ITreeNode _root;
    Dictionary<AllyStates, float> _randomWS = new Dictionary<AllyStates, float>();
    private AllyStayState<AllyStates> _stayState;


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
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flockManager, _maxFidelity);
        var stay = new AllyStayState<AllyStates>(_maxTimeScare,AllyStates.Default,_flockManager);
        var reentry = new ReentryState<AllyStates>();
        var die = new AllyStateDie<AllyStates>();
       
        
        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        stay.InitializedState(_model, _view, _fsm);
        reentry.InitializedState(_model, _view, _fsm);

        ///Timers States
        _stayTimer = stay.CurrentTimer;
        _stayState = stay;

        ///add transitions
        /// /walk
        walk.AddTransition(AllyStates.Follow, follow);
        walk.AddTransition(AllyStates.Reentry, reentry);

        /// /follow
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Stay, stay);

        /// /stay
        stay.AddTransition(AllyStates.Walk, walk);
        stay.AddTransition(AllyStates.Follow, follow);
        stay.AddTransition(AllyStates.Reentry, reentry);

        /// /reentry*
        reentry.AddTransition(AllyStates.Stay, stay);
        reentry.AddTransition(AllyStates.Walk, walk);
        reentry.AddTransition(AllyStates.Die, die);




        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        TreeAction walk = new TreeAction(ActionWalk);
        TreeAction follow = new TreeAction(ActionFollow);
        TreeAction stay = new TreeAction(ActionStay);
        TreeAction die = new TreeAction(ActionDie);
        TreeAction again = new TreeAction(ActionAgain,AllyStates.Reentry);
        TreeAction randomWS = new TreeAction(ActionRandomWS);

        _randomWS.Add(AllyStates.Reentry, _model._fidelity);
        _randomWS.Add(AllyStates.Walk, _model._fidelity);
        _randomWS.Add(AllyStates.Die, _model._alliesNear);

        ///Q = termino el stay?
        TreeQuestion isStayOver = new TreeQuestion(IsStayOver, randomWS, stay);
        ///Q = tiene muchos lideres?
        TreeQuestion hasManyLeaders = new TreeQuestion(HasManyLeaders, isStayOver, follow);
        ///Q = tengo un lider?
        TreeQuestion hasLeader = new TreeQuestion(HasLeader, hasManyLeaders, walk);
        _root = hasLeader;
    }
    bool IsStayOver()
    {
        _stayTimer = _stayState.CurrentTimer;

        return _stayTimer < 1 && !_fsm.StateRepeat;

    }
    bool HasManyLeaders()
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
        
        _randomWS[AllyStates.Reentry] = Mathf.Clamp01(_model._fidelity);
        _randomWS[AllyStates.Walk] = 1f - Mathf.Clamp01(_model._fidelity + 1);
        _randomWS[AllyStates.Die] = 1f - Mathf.Clamp01(_model._alliesNear);

        _currentState = MyRandoms.Roulette(_randomWS);
        _fsm.Transitions(_currentState);

    }
    public void ActionWalk()
    {
        _currentState = AllyStates.Walk;
        _fsm.Transitions(_currentState);
    }
    public void ActionAgain()
    {
        _currentState = AllyStates.Reentry;
        _fsm.Transitions(_currentState);
       
    }
    public void ActionStay()
    {
        _currentState = AllyStates.Stay;
        _fsm.Transitions(_currentState);
    }
    public void ActionFollow()
    {
        _currentState = AllyStates.Follow;
        _fsm.Transitions(_currentState);
    }
    public void ActionDie()
    {
        _currentState = AllyStates.Die;
        _fsm.Transitions(_currentState);
    }

    private void Update()
    {
        Debug.Log("[STATE REPEAT: ]"+_fsm.StateRepeat);
        _fsm.OnUpdate();
        _root.Execute();     
    }

}