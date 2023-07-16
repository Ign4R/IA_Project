using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour
{
    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public PlayerModel _target;
    public float _timePredict = 1f;
    public LeaderModel _model;
    private LeaderView _view;
    FSM<LeaderStateEnum> _fsm;
    ITreeNode _root;
    Dictionary<Type, ISteering>_steerings = new Dictionary<Type, ISteering>(); 
    private void Awake()
    {
        _model = GetComponentInChildren<LeaderModel>();
        _view= _model.GetComponentInChildren<LeaderView>();
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
        var steel = new StealState<LeaderStateEnum>(_steerings[pursuit],_steerings[obsAvoid]);
        var exploration = new ExplorationState<LeaderStateEnum>(_nodeGrid, _model._startNode, _steerings[obsAvoid]); ///*Primero creo
        //var attack = new EnemyAttackState<EnemyStateEnum>(_model._setAttackTimer, _steerings[pursuit]);
        var targetFind = new TargetFindState<LeaderStateEnum>(_steerings[obsAvoid], _nodeGrid, _target.transform, _model._setHuntTimer);

        exploration.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        steel.InitializedState(_model, _view, _fsm);
        targetFind.InitializedState(_model, _view, _fsm);
        idle.InitializedState(_model, _view, _fsm);

        ///idle
        idle.AddTransition(LeaderStateEnum.Exploring, exploration);
        idle.AddTransition(LeaderStateEnum.Chasing, steel);

        ///patrol 
        exploration.AddTransition(LeaderStateEnum.Idle, idle);
        exploration.AddTransition(LeaderStateEnum.Chasing, steel);
        exploration.AddTransition(LeaderStateEnum.Finding, targetFind);

        ///chase
        steel.AddTransition(LeaderStateEnum.Exploring, exploration);
        steel.AddTransition(LeaderStateEnum.Idle, idle);
        steel.AddTransition(LeaderStateEnum.Finding, targetFind);
        ///hunt
        targetFind.AddTransition(LeaderStateEnum.Exploring, exploration);
        targetFind.AddTransition(LeaderStateEnum.Idle, idle);
        targetFind.AddTransition(LeaderStateEnum.Chasing, steel);

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
        var isCanFind = new TreeQuestion(IsCanFind, find, exploration);
        /// esta el ataque activado?
        var IsOnAttack = new TreeQuestion(null, null, chase);
        ///se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight, IsOnAttack, isCanFind);
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
        _fsm.Transitions(LeaderStateEnum.Idle);
    }

    void ActionExploration()
    {
        _fsm.Transitions(LeaderStateEnum.Exploring);
    }
    void ActionPursuit()
    {
        _fsm.Transitions(LeaderStateEnum.Chasing);

    }
    void ActionFind()
    {
        _fsm.Transitions(LeaderStateEnum.Finding);

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
    //si no añadis una transcision por ej de patrol a attack.
    //pero si añadis de chase a attack (cuando pases de patrol a chase) pasaras luego a attack?
    //pero si o si debes pasar primero por chase?*/
    //

}