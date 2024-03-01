using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public int _maxFidelity = 10;
    [ReadOnly][SerializeField] private float _scaredTimer;
    [ReadOnly] [SerializeField] private AllyStates _currentState;
    public float _maxTimeScare;
    public Leadership _leadership;
    public float _maxDistance;
    public AllyModel _model;
    public AllyView _view;
    FSM<AllyStates> _fsm;
    private FlockingManager _flk;
    ITreeNode _root;
    private AllyScaredState<AllyStates> _stayState;
    private RandomWheelSelection<AllyStates> _RWS;


    private void Awake()
    {
        _model = GetComponent<AllyModel>();
        _view = GetComponent<AllyView>();
        _leadership = GetComponent<Leadership>();
        _flk = GetComponent<FlockingManager>();
        InitializedFSM();
        InitializeTree();
    }
    private void Start()
    {
        _model.OnDie += _view.OnDie;
        _model.OnDie += ActionDie;
    }

    
    public void InitializedFSM()
    {
        _fsm = new FSM<AllyStates>();
        var avoid = new ObstacleAvoidance(_model.transform, _flk.maskBoids, _model._maxObs, _model._angleAvoid, _model._radius);
        var aimless = new AllyAimlesslyState<AllyStates>();
        var follow = new AllyFollowState<AllyStates>(AllyStates.Idle, _flk, _maxFidelity, avoid,_maxDistance);
        var scared = new AllyScaredState<AllyStates>(_maxTimeScare,AllyStates.Default,_flk);
        var die = new AllyStateDie<AllyStates>();
       _RWS=new RandomWheelSelection<AllyStates>();
        
        _RWS.InitializedState(_model, _view, _fsm);
        aimless.InitializedState(_model, _view, _fsm);
        follow.InitializedState(_model, _view, _fsm);
        scared.InitializedState(_model, _view, _fsm);
        die.InitializedState(_model, _view, _fsm);


        ///Timers States
        _scaredTimer = scared.CurrentTimer;
        _stayState = scared;

        ///add transitions
        /// /aimless sin rumbo
        aimless.AddTransition(AllyStates.Follow, follow);
        aimless.AddTransition(AllyStates.RandomWS, _RWS);

        /// /follow
        follow.AddTransition(AllyStates.Aimless, aimless);
        follow.AddTransition(AllyStates.Scared, scared);

        /// /stay
        scared.AddTransition(AllyStates.Aimless, aimless);
        scared.AddTransition(AllyStates.Follow, follow);
        scared.AddTransition(AllyStates.RandomWS, _RWS);

        /// /randomWS*
        _RWS.AddTransition(AllyStates.Scared, scared);
        _RWS.AddTransition(AllyStates.Aimless, aimless);
        _RWS.AddTransition(AllyStates.Die, die);




        _fsm.SetInit(aimless);
    }

    public void InitializeTree()
    {
        TreeAction aimless = new TreeAction(ActionAimless);
        TreeAction follow = new TreeAction(ActionFollow);
        TreeAction scared = new TreeAction(ActionScared);
        TreeAction die = new TreeAction(ActionDie);
        TreeAction randomWS = new TreeAction(RWS);
        TreeAction selected = new TreeAction(ActionSelected);
     


        ///Q = ........?
        TreeQuestion isSelectDone = new TreeQuestion(IsSelectDone, selected, randomWS);
        ///Q = termino el stay?
        TreeQuestion isScaredOver = new TreeQuestion(IsScaredOver, isSelectDone, scared);
        ///Q = hay peligro?
        TreeQuestion isInDanger = new TreeQuestion(IsInDanger, isScaredOver, follow);
        ///Q = tengo un lider?
        TreeQuestion hasLeader = new TreeQuestion(HasLeader, isInDanger, aimless);
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
    bool IsScaredOver()
    {
        _scaredTimer = _stayState.CurrentTimer;
        return _scaredTimer < 1;

    }
    bool IsInDanger()
    {
        var leads= _model._leaders;
        return leads.Count > 1 || _model.isScared;
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
        if (_currentState == AllyStates.Scared) return;
        _RWS._selectDone = true;
    }
    public void ActionAimless()
    {
        _currentState = AllyStates.Aimless;
        _fsm.Transitions(_currentState);
    }

    public void ActionScared()
    {
        _currentState = AllyStates.Scared;
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

        _fsm.OnUpdate();
        _root.Execute();     
    }

}