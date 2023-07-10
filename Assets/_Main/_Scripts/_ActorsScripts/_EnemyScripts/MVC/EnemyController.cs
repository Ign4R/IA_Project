using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask mask;
    public float angle;
    public float radius;
    public int maxObs;
    public NodeGrid _nodeGrid;
    public float maxRandomTime = 20;
    public PlayerModel _target;
    public float _timePredict = 1f;
    public EnemyModel _model;
    public EnemyView _view;
    FSM<EnemyStateEnum> _fsm;
    ITreeNode _root;
    private void Awake()
    {     
        InitializedEvents();
        InitializedFSM();
        InitializeTree();
    }
    private void Start()
    {
        var startNode = _model._startNode.transform.position;
        startNode.y = transform.localPosition.y;
        transform.position = startNode;
    }
    void InitializedEvents()
    {
        _model.OnAttack += _view.AnimAttack;

    }

    //TODO
    //[DUDA]
    //si no añadis una transcision por ej de patrol a attack.
    //pero si añadis de chase a attack (cuando pases de patrol a chase) pasaras luego a attack?
    //pero si o si debes pasar primero por chase?*/
    //
    void InitializedFSM()
    {


        _fsm = new FSM<EnemyStateEnum>();

        ISteering pursuit = new Pursuit(_model.transform, _target, _timePredict); ///*PRIORITY:BAJA-(crearlo en otro lado)

        var chase = new EnemyChaseState<EnemyStateEnum>(pursuit);
        var idle = new EnemyIdleState<EnemyStateEnum>();
        var patrol = new EnemyPatrolState<EnemyStateEnum>(_nodeGrid, _model._startNode); ///*Primero creo
        var attack = new EnemyAttackState<EnemyStateEnum>(_model._setAttackTimer, pursuit);
        var hunt = new EnemyHuntState<EnemyStateEnum>(EnemyStateEnum.Patrolling, _nodeGrid, _target.transform,_model._setHuntTimer);

        idle.InitializedState(_model, _view, _fsm);
        patrol.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        attack.InitializedState(_model, _view, _fsm);
        chase.InitializedState(_model, _view, _fsm);
        hunt.InitializedState(_model, _view, _fsm);

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

        ///questions

        /// esta en busqueda?
        var isOnHunt = new TreeQuestion(IsOnHunt, hunt, patrol);
        /// esta el ataque activado?
        var isOnAttack = new TreeQuestion(IsOnAttack, attack, chase);
        ///se ve el objetivo?
        var isInSight = new TreeQuestion(IsInSight, isOnAttack, isOnHunt);
        /// termino el juego?
        var isFinishGame = new TreeQuestion(IsFinishGame, idle, isInSight);

        _root = isFinishGame;

    }

    bool IsOnAttack()
    {
        ///El ataque va a depender de si puede atacar o el ataque esta activo y y si el rayo lo detecta
        ///si (/*puede atacar*/ |o| /* la duracion del ataque esta activa*/) y el rayo detecta?
        return ((_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform));
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



}