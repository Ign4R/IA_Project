using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public int _maxFidelity = 10;
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
    private AllyStayState<AllyStates> _stayState;
    private RandomWheelSelection<AllyStates> _RWS;


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
        var die = new AllyStateDie<AllyStates>();
       _RWS=new RandomWheelSelection<AllyStates>();
        
        _RWS.InitializedState(_model, _view, _fsm);
        walk.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        stay.InitializedState(_model, _view, _fsm);


        ///Timers States
        _stayTimer = stay.CurrentTimer;
        _stayState = stay;

        ///add transitions
        /// /walk
        walk.AddTransition(AllyStates.Follow, follow);
        walk.AddTransition(AllyStates.RandomWS, _RWS);

        /// /follow
        follow.AddTransition(AllyStates.Walk, walk);
        follow.AddTransition(AllyStates.Stay, stay);

        /// /stay
        stay.AddTransition(AllyStates.Walk, walk);
        stay.AddTransition(AllyStates.Follow, follow);
        stay.AddTransition(AllyStates.RandomWS, _RWS);

        /// /randomWS*
        _RWS.AddTransition(AllyStates.Stay, stay);
        _RWS.AddTransition(AllyStates.Walk, walk);
        _RWS.AddTransition(AllyStates.Die, die);




        _fsm.SetInit(walk);
    }

    public void InitializeTree()
    {
        TreeAction walk = new TreeAction(ActionWalk);
        TreeAction follow = new TreeAction(ActionFollow);
        TreeAction stay = new TreeAction(ActionStay);
        TreeAction die = new TreeAction(ActionDie);
        TreeAction randomWS = new TreeAction(RWS);
        TreeAction selected = new TreeAction(ActionSelected);
     


        ///Q = ........?
        TreeQuestion isSelectDone = new TreeQuestion(IsSelectDone, selected, randomWS);
        ///Q = termino el stay?
        TreeQuestion isStayOver = new TreeQuestion(IsStayOver, isSelectDone, stay);
        ///Q = hay riesgo?
        TreeQuestion isInRisk = new TreeQuestion(IsInRisk, isStayOver, follow);
        ///Q = tengo un lider?
        TreeQuestion hasLeader = new TreeQuestion(HasLeader, isInRisk, walk);
        _root = hasLeader;
    }


    bool IsSelectDone()
    {
        if (_RWS._selectDone)
        {
            _RWS._selectDone = false;
            return true; 
        }
        return false;
    }
    bool IsStayOver()
    {
        _stayTimer = _stayState.CurrentTimer;
        return _stayTimer < 1;

    }
    bool IsInRisk()
    {
        List<NPCLeader_M> leads= _model._leaders;
        return leads.Count > 1 || _model.InRisk;
    }
    bool HasLeader()
    {
        return _model.HasLeader;
    }
    public void RWS()
    {
        _currentState = AllyStates.RandomWS;
        _fsm.Transitions(_currentState);
    }
    public void ActionSelected()
    {
        _currentState = _RWS.Result;
        _fsm.Transitions(_currentState);
        if (_currentState == AllyStates.Stay) return;
        _RWS._selectDone = true;
    }
    public void ActionWalk()
    {
        _currentState = AllyStates.Walk;
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
        Debug.Log("[STATE RANDOM: ]"+_RWS.Result);
        Debug.Log("[RWS Stop: ]" + _RWS._selectDone);
        _fsm.OnUpdate();
        _root.Execute();     
    }

}