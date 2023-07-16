using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLeader_C : MonoBehaviour
{
    public float _timerExploration;
    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public PlayerModel _target;
    public float _timePredict = 1f;
    public NPCLeader_M _model;
    private NPCLeader_V _view;
    FSM<NPCLeaderStateEnum> _fsm;
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

        _fsm = new FSM<NPCLeaderStateEnum>();
        var idle = new EnemyIdleState<NPCLeaderStateEnum>();
        var steel = new StealState<NPCLeaderStateEnum>(_steerings[pursuit],_steerings[obsAvoid]);
        var exploration = new ExplorationState<NPCLeaderStateEnum>(_timerExploration,_nodeGrid, _model._startNode, _steerings[obsAvoid]); ///*Primero creo
        //var attack = new EnemyAttackState<EnemyStateEnum>(_model._setAttackTimer, _steerings[pursuit]);
        //var targetFind = new TargetFindState<NPCLeaderStateEnum>(_steerings[obsAvoid], _nodeGrid, _target.transform, _model._setHuntTimer);

        exploration.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        steel.InitializedState(_model, _view, _fsm);
        //targetFind.InitializedState(_model, _view, _fsm);
        idle.InitializedState(_model, _view, _fsm);

        ///idle
        idle.AddTransition(NPCLeaderStateEnum.Exploring, exploration);
        idle.AddTransition(NPCLeaderStateEnum.Chasing, steel);

        ///patrol 
        exploration.AddTransition(NPCLeaderStateEnum.Idle, idle);
        exploration.AddTransition(NPCLeaderStateEnum.Chasing, steel);
        //exploration.AddTransition(NPCLeaderStateEnum.Finding, targetFind);

        ///chase
        steel.AddTransition(NPCLeaderStateEnum.Exploring, exploration);
        steel.AddTransition(NPCLeaderStateEnum.Idle, idle);
        //steel.AddTransition(NPCLeaderStateEnum.Finding, targetFind);
        ///hunt
        //targetFind.AddTransition(NPCLeaderStateEnum.Exploring, exploration);
        //targetFind.AddTransition(NPCLeaderStateEnum.Idle, idle);
        //targetFind.AddTransition(NPCLeaderStateEnum.Chasing, steel);

        _fsm.SetInit(exploration);
    }

    public void InitializeTree()
    {
        ///actions
        var idle = new TreeAction(ActionIdle);
        var exploration = new TreeAction(ActionExploration);
        var chase = new TreeAction(ActionPursuit);
        var find = new TreeAction(ActionFind);
        var _default = new TreeAction(default);



        ///questions

        /// puedo buscar?
        //var isCanFind = new TreeQuestion(IsCanFind, find, exploration);
        /// esta el ataque activado?
        //var IsOnAttack = new TreeQuestion(null, null, chase);
        ///se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight,chase , exploration);
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
    bool IsCanFind()
    {
        return _model.SpottedTarget;
    }
    bool IsInSight()
    {
        return _model.CheckRange(_target.transform) && _model.CheckAngle(_target.transform) && _model.CheckView(_target.transform);
    }
    void ActionIdle()
    {
        _fsm.Transitions(NPCLeaderStateEnum.Idle);
    }

    void ActionExploration()
    {
        _fsm.Transitions(NPCLeaderStateEnum.Exploring);
    }
    void ActionPursuit()
    {
        _fsm.Transitions(NPCLeaderStateEnum.Chasing);

    }
    void ActionFind()
    {
        _fsm.Transitions(NPCLeaderStateEnum.Finding);

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

    //TODO
    //[DUDA]
    //si no a�adis una transcision por ej de patrol a attack.
    //pero si a�adis de chase a attack (cuando pases de patrol a chase) pasaras luego a attack?
    //pero si o si debes pasar primero por chase?*/
    //

}