using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public Node _safeZone;
    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public BaseModel _target;
    public float _timePredict = 1f;
    public SpiderModel _model;
    private SpiderView _view;
    FSM<SpiderStateEnum> _fsm;
    [ReadOnly] public SpiderStateEnum _currentState;
    ITreeNode _root;
    Dictionary<Type, ISteering>_steerings = new Dictionary<Type, ISteering>(); 
    private void Awake()
    {
        _model = GetComponentInChildren<SpiderModel>();
        _view= _model.GetComponentInChildren<SpiderView>();
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
        _model.OnAttack += _view.AttackAnim;
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

        _fsm = new FSM<SpiderStateEnum>();
        var idle = new EnemyIdleState<SpiderStateEnum>();
        var chase = new ChaseState<SpiderStateEnum>(_steerings[pursuit],_steerings[obsAvoid]);
        var exploration = new ExplorationState<SpiderStateEnum>(_nodeGrid, _model._startNode, _steerings[obsAvoid]); ///*Primero creo
        var attack = new EnemyAttackState<SpiderStateEnum>(_model._setAttackTimer,_steerings[pursuit]);
        var findHome = new HunterState<SpiderStateEnum>(_steerings[obsAvoid],_nodeGrid,_target.transform,_model._setHuntTimer);

        exploration.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        chase.InitializedState(_model, _view, _fsm);
        findHome.InitializedState(_model, _view, _fsm);
        idle.InitializedState(_model, _view, _fsm);
        attack.InitializedState(_model, _view, _fsm);

        ///idle
        idle.AddTransition(SpiderStateEnum.Exploring, exploration);
        idle.AddTransition(SpiderStateEnum.Chasing, chase);
        ///patrol 
        exploration.AddTransition(SpiderStateEnum.Idle, idle);
        exploration.AddTransition(SpiderStateEnum.Chasing, chase);
        exploration.AddTransition(SpiderStateEnum.Finding, findHome);
        exploration.AddTransition(SpiderStateEnum.Attacking, attack);

        ///chase
        chase.AddTransition(SpiderStateEnum.Exploring, exploration);
        chase.AddTransition(SpiderStateEnum.Idle, idle);
        chase.AddTransition(SpiderStateEnum.Finding, findHome);
        chase.AddTransition(SpiderStateEnum.Attacking, attack);
        ///findZone
        findHome.AddTransition(SpiderStateEnum.Exploring, exploration);
        findHome.AddTransition(SpiderStateEnum.Idle, idle);
        findHome.AddTransition(SpiderStateEnum.Chasing, chase);
        findHome.AddTransition(SpiderStateEnum.Attacking, attack);
        ///attack
        attack.AddTransition(SpiderStateEnum.Exploring, exploration);
        attack.AddTransition(SpiderStateEnum.Idle, idle);
        attack.AddTransition(SpiderStateEnum.Chasing, chase);

        _fsm.SetInit(exploration);
    }

    public void InitializeTree()
    {
        ///actions
        var idle = new TreeAction(ActionIdle);
        var exploration = new TreeAction(ActionExploration);
        var chase = new TreeAction(ActionPursuit);
        var hunt = new TreeAction(ActionFindHome);
        var _default = new TreeAction(default);
        var attack= new TreeAction(ActionAttack);

        ///questions
        /// esta en busqueda?
        var isOnHunt = new TreeQuestion(IsOnHunt, hunt, exploration);
        /// esta el ataque activado?
        var isOnAttack = new TreeQuestion(IsNearTarget, attack, chase);
        ///se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight, isOnAttack, isOnHunt);
        /// termino el juego?
        var isEndGame = new TreeQuestion(IsOverGame, idle, isInSight);
        /// el estado actual es null?
        var isNullState = new TreeQuestion(IsNullState, _default, isEndGame);
        _root = isNullState;

    }

    bool IsNearTarget()
    {
        return (_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform);
    }
    bool IsNullState()
    {
        /// si el estado de la fsm es null, no necesito seguir haciendo el arbol, ya que aunque lo mande a default. Transitions se seguira reporduciendo
        return _fsm.Current == null;
    }
    bool IsOverGame()
    {
        return GameManager.Instance.FinishGame;
    }
    bool IsOnHunt()
    {
        return _model.SpottedTarget;
    }

    bool IsInSight()
    {
        return _model.CheckRange(_target.transform) && _model.CheckAngle(_target.transform) && _model.CheckView(_target.transform);
    }
    void ActionIdle()
    {
        _currentState = SpiderStateEnum.Idle;
        _fsm.Transitions(_currentState);
    }

    void ActionExploration()
    {
        _currentState = SpiderStateEnum.Exploring;
        _fsm.Transitions(_currentState);
    }
    void ActionPursuit()
    {
        _currentState = SpiderStateEnum.Chasing;
        _fsm.Transitions(_currentState);
    }
    void ActionFindHome()
    {
        _currentState = SpiderStateEnum.Finding;
        _fsm.Transitions(_currentState);
    }

    void ActionAttack()
    {
        _currentState = SpiderStateEnum.Attacking;
        _fsm.Transitions(_currentState);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_model.transform.position, _model._radiusAvoid);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(_model.transform.position, Quaternion.Euler(0, _model._angleAvoid / 2, 0) * _model.GetForward* _model._radiusAvoid);
        Gizmos.DrawRay(_model.transform.position, Quaternion.Euler(0, -_model._angleAvoid / 2, 0) * _model.GetForward * _model._radiusAvoid);

    }

  

}