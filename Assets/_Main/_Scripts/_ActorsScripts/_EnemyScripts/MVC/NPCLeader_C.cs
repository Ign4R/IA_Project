using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLeader_C : MonoBehaviour
{
    public LayerMask _allyMask;
    public Node _safeZone;
    public float _timerSteal;
    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public NPCLeader_M _target;
    public float _timePredict = 1f;
    public NPCLeader_M _model;
    private NPCLeader_V _view;
    FSM<LeaderStateEnum> _fsm;
    [ReadOnly] public LeaderStateEnum _currentState;
    ITreeNode _root;
    Dictionary<Type, ISteering>_steerings = new Dictionary<Type, ISteering>(); 
    private void Awake()
    {
        _model = GetComponentInChildren<NPCLeader_M>();
        _view= _model.GetComponentInChildren<NPCLeader_V>();
    }
    private void Start()
    {
        var startNode = _model._startNode.transform.position;
        startNode.y = transform.localPosition.y;
        transform.position = startNode;
        InitializedSteering();
        InitializedFSM();
        InitializeTree();
        InitializedEvents();

    }
    void InitializedEvents()
    {
        _model.OnAttack += _view.AnimAttack;

    }

    void InitializedSteering()
    {
        var obsAvoid = new ObstacleAvoidance(_model.transform, _model._maskAvoid, _model._maxObs, _model._angleAvoid, _model._radiusAvoid);
        var pursuit = new Pursuit(_model.transform, _target, _timePredict); 
        _steerings.Add(obsAvoid.GetType(), obsAvoid);
        _steerings.Add(pursuit.GetType(), pursuit);
    }

    void InitializedFSM()
    {
        var pursuit = typeof(Pursuit);
        var obsAvoid = typeof(ObstacleAvoidance);

        _fsm = new FSM<LeaderStateEnum>();
        var idle = new EnemyIdleState<LeaderStateEnum>();
        var chase = new ChaseState<LeaderStateEnum>(_steerings[pursuit],_steerings[obsAvoid]);
        var exploration = new ExplorationState<LeaderStateEnum>(_nodeGrid, _model._startNode, _steerings[obsAvoid]); ///*Primero creo
        var steal = new StealState<LeaderStateEnum>(_timerSteal, _allyMask);
        var targetFind = new TargetFindState<LeaderStateEnum>(_steerings[obsAvoid], _nodeGrid, _safeZone);

        exploration.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        chase.InitializedState(_model, _view, _fsm);
        targetFind.InitializedState(_model, _view, _fsm);
        idle.InitializedState(_model, _view, _fsm);
        steal.InitializedState(_model, _view, _fsm);

        ///idle
        idle.AddTransition(LeaderStateEnum.Exploring, exploration);
        idle.AddTransition(LeaderStateEnum.Chasing, chase);

        ///patrol 
        exploration.AddTransition(LeaderStateEnum.Idle, idle);
        exploration.AddTransition(LeaderStateEnum.Chasing, chase);
        exploration.AddTransition(LeaderStateEnum.Finding, targetFind);
        exploration.AddTransition(LeaderStateEnum.Stealing, steal);

        ///chase
        chase.AddTransition(LeaderStateEnum.Exploring, exploration);
        chase.AddTransition(LeaderStateEnum.Idle, idle);
        chase.AddTransition(LeaderStateEnum.Finding, targetFind);
        chase.AddTransition(LeaderStateEnum.Stealing, steal);
        ///findZone
        targetFind.AddTransition(LeaderStateEnum.Exploring, exploration);
        targetFind.AddTransition(LeaderStateEnum.Idle, idle);
        targetFind.AddTransition(LeaderStateEnum.Chasing, chase);
        targetFind.AddTransition(LeaderStateEnum.Stealing, steal);
        ///steal
        steal.AddTransition(LeaderStateEnum.Exploring, exploration);
        steal.AddTransition(LeaderStateEnum.Idle, idle);
        steal.AddTransition(LeaderStateEnum.Chasing, chase);

        _fsm.SetInit(exploration);
    }

    public void InitializeTree()
    {
        ///actions
        var idle = new TreeAction(ActionIdle);
        var exploration = new TreeAction(ActionExploration);
        var chase = new TreeAction(ActionPursuit);
        var findSafeZone = new TreeAction(ActionFind);
        var _default = new TreeAction(default);
        var steal= new TreeAction(ActionSteal);



        ///questions

        var canSteal = new TreeQuestion(CanSteel, steal, chase);
        /// estoy en peligro?
        var isAtRisk = new TreeQuestion(IsAtRisk, findSafeZone, exploration);
        /// se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight, chase, isAtRisk);
        /// se acabo el juego?
        var isOverGame = new TreeQuestion(IsOverGame, idle, isInSight);
        /// el estado actual es null?
        var isNullState = new TreeQuestion(IsNullState, _default, isOverGame);
        _root = isNullState;

    }

    //bool IsOnAttack()
    //{
    //    ///El ataque va a depender de si puede atacar o el ataque esta activo y y si el rayo lo detecta
    //    ///si (/*puede atacar*/ |o| /* la duracion del ataque esta activa*/) y el rayo detecta?
    //    return ((_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform));
    //}
    bool IsNullState()
    {
        /// si el estado de la fsm es null, no necesito seguir haciendo el arbol, ya que aunque lo mande a default. Transitions se seguira reporduciendo
        return _fsm.Current == null;
    }
    bool IsOverGame()
    {
        return GameManager.Instance.FinishGame;
    }
    bool IsAtRisk()
    {
        return _model.isVulnerable && _model.isTargetSpotted;
    }
    bool CanSteel()
    {
        return Vector3.Distance(_model.transform.position, _target.transform.position) <= 17;
    }
    bool IsInSight()
    {
        if (_model.CheckRange(_target.transform) && _model.CheckAngle(_target.transform) && _model.CheckView(_target.transform))
        {
            _model.isTargetSpotted = true;
            return !_model.isVulnerable;

        }
        return false;
    }
    void ActionIdle()
    {
        _currentState = LeaderStateEnum.Idle;
        _fsm.Transitions(_currentState);
    }

    void ActionExploration()
    {
        _currentState = LeaderStateEnum.Exploring;
        _fsm.Transitions(_currentState);
    }
    void ActionPursuit()
    {
        _currentState = LeaderStateEnum.Chasing;
        _fsm.Transitions(_currentState);
    }
    void ActionFind()
    {
        _currentState = LeaderStateEnum.Finding;
        _fsm.Transitions(_currentState);
    }

    void ActionSteal()
    {
        _currentState = LeaderStateEnum.Stealing;
        _fsm.Transitions(_currentState);
    }
    private void Update()
    {

        _fsm.OnUpdate();
        _root.Execute();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_model.transform.position, _model._radiusAvoid);
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(_model.transform.position, Quaternion.Euler(0, _model._angleAvoid / 2, 0) * _model.GetForward* _model._radiusAvoid);
        Gizmos.DrawRay(_model.transform.position, Quaternion.Euler(0, -_model._angleAvoid / 2, 0) * _model.GetForward * _model._radiusAvoid);

    }

  

}