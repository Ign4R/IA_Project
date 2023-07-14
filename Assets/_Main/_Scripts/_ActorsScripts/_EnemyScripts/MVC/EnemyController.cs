using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public PlayerModel _target;
    public float _timePredict = 1f;
    public EnemyModel _model;
    private EnemyView _view;
    FSM<EnemyStateEnum> _fsm;
    ITreeNode _root;
    ISteering _obsAvoidance;
    Dictionary<Type, ISteering>_steerings = new Dictionary<Type, ISteering>(); 
    private void Awake()
    {
        _model = GetComponentInChildren<EnemyModel>();
        _view= _model.GetComponentInChildren<EnemyView>();
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

    //public ISteering GetSteering<Type>() where Type : ISteering
    //{
    //    foreach (var steering in _steerings)
    //    {
    //        if (steering.GetType() is Type)
    //        {
    //            return steering;
    //        }
    //    }
    //    return null;
    //}
    void InitializedFSM()
    {
        var pursuit = typeof(Pursuit);
        var obsAvoid = typeof(ObstacleAvoidance);

        _fsm = new FSM<EnemyStateEnum>();
        var nav = new NavigationState<EnemyStateEnum>(_steerings[obsAvoid]);
        var idle = new EnemyIdleState<EnemyStateEnum>();
        var chase = new EnemyChaseState<EnemyStateEnum>(_steerings[pursuit],_steerings[obsAvoid]);
        var patrol = new EnemyPatrolState<EnemyStateEnum>(_nodeGrid, _model._startNode, _steerings[obsAvoid]); ///*Primero creo
        var attack = new EnemyAttackState<EnemyStateEnum>(_model._setAttackTimer, _steerings[pursuit]);
        var hunt = new EnemyHuntState<EnemyStateEnum>(EnemyStateEnum.Patrolling, _steerings[obsAvoid], _nodeGrid, _target.transform, _model._setHuntTimer);

        patrol.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        attack.InitializedState(_model, _view, _fsm);
        chase.InitializedState(_model, _view, _fsm);
        hunt.InitializedState(_model, _view, _fsm);
        idle.InitializedState(_model, _view, _fsm);



        ///idle
        idle.AddTransition(EnemyStateEnum.Patrolling, patrol);
        idle.AddTransition(EnemyStateEnum.Attacking, attack);
        idle.AddTransition(EnemyStateEnum.Chasing, chase);



        ///patrol 
        patrol.AddTransition(EnemyStateEnum.Idle, idle);
        patrol.AddTransition(EnemyStateEnum.Attacking, attack);
        patrol.AddTransition(EnemyStateEnum.Chasing, chase);
        patrol.AddTransition(EnemyStateEnum.Hunting, hunt);
        ///attack
        attack.AddTransition(EnemyStateEnum.Idle, idle);
        attack.AddTransition(EnemyStateEnum.Patrolling, patrol);
        attack.AddTransition(EnemyStateEnum.Chasing, chase);
        attack.AddTransition(EnemyStateEnum.Hunting, hunt);
        ///chase
        chase.AddTransition(EnemyStateEnum.Patrolling, patrol);
        chase.AddTransition(EnemyStateEnum.Attacking, attack);
        chase.AddTransition(EnemyStateEnum.Idle, idle);
        chase.AddTransition(EnemyStateEnum.Hunting, hunt);
        ///hunt
        hunt.AddTransition(EnemyStateEnum.Patrolling, patrol);
        hunt.AddTransition(EnemyStateEnum.Attacking, attack);
        hunt.AddTransition(EnemyStateEnum.Idle, idle);
        hunt.AddTransition(EnemyStateEnum.Chasing, chase);

        _fsm.SetInit(patrol);
    }

    public void InitializeTree()
    {
        ///actions
        var idle = new TreeAction(ActionIdle);
        var patrol = new TreeAction(ActionPatrol);
        var attack = new TreeAction(ActionAttack);
        var chase = new TreeAction(ActionPursuit);
        var hunt = new TreeAction(ActionHunt);
        var _default = new TreeAction(default);



        ///questions

        /// esta en busqueda?
        var isOnHunt = new TreeQuestion(IsOnHunt, hunt, patrol);
        /// esta el ataque activado?
        var isOnAttack = new TreeQuestion(IsOnAttack, attack, chase);
        ///se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight, isOnAttack, isOnHunt);
        /// termino el juego?
        var isEndGame = new TreeQuestion(IsFinishGame, idle, isInSight);
        /// el estado actual es null?
        var isNullState = new TreeQuestion(IsNullState, _default, isEndGame);
        _root = isNullState;

    }

    bool IsOnAttack()
    {
        ///El ataque va a depender de si puede atacar o el ataque esta activo y y si el rayo lo detecta
        ///si (/*puede atacar*/ |o| /* la duracion del ataque esta activa*/) y el rayo detecta?
        return ((_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform));
    }
    bool IsNullState()
    {
        /// si el estado de la fsm es null, no necesito seguir haciendo el arbol, ya que aunque lo mande a default. Transitions se seguira reporduciendo
        return _fsm.Current == null;
    }
    bool IsFinishGame()
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
        _fsm.Transitions(EnemyStateEnum.Idle);
    }

    void ActionPatrol()
    {
        _fsm.Transitions(EnemyStateEnum.Patrolling);
    }
    void ActionPursuit()
    {
        _fsm.Transitions(EnemyStateEnum.Chasing);

    }
    void ActionHunt()
    {
        _fsm.Transitions(EnemyStateEnum.Hunting);

    }
    void ActionAttack()
    {
        
        _fsm.Transitions(EnemyStateEnum.Attacking);
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