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
    FSM<EnemyStatesEnum> _fsm;
    ITreeNode _root;
    private void Awake()
    {
        InitializedEvents();
        InitializedFSM();
        InitializeTree();
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
        _fsm = new FSM<EnemyStatesEnum>();

        ISteering pursuit = new Pursuit(_model.transform, _target, _timePredict); ///*PRIORITY:BAJA-(crearlo en otro lado)

        var chase = new EnemyStateChase<EnemyStatesEnum>(pursuit);
        var idle = new EnemyStateIdle<EnemyStatesEnum>(EnemyStatesEnum.Patrolling);
        var patrol = new EnemyStatePatrol<EnemyStatesEnum>(_nodeGrid, _model._startNode); ///*Primero creo
        var attack = new EnemyStateAttack<EnemyStatesEnum>(_model._setAttackTimer);
        var hunt = new EnemyStateHunt<EnemyStatesEnum>(EnemyStatesEnum.Patrolling, _nodeGrid, _target.transform,_model._setHuntTimer);

        idle.InitializedState(_model, _view, _fsm);
        patrol.InitializedState(_model, _view, _fsm);///*Luego llamo y le doy referencia al model
        attack.InitializedState(_model, _view, _fsm);
        chase.InitializedState(_model, _view, _fsm);
        hunt.InitializedState(_model, _view, _fsm);

        ///idle
        idle.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        idle.AddTransition(EnemyStatesEnum.Attacking, attack);
        idle.AddTransition(EnemyStatesEnum.Chasing, chase);
        ///patrol 
        patrol.AddTransition(EnemyStatesEnum.Idle, idle);
        patrol.AddTransition(EnemyStatesEnum.Attacking, attack);
        patrol.AddTransition(EnemyStatesEnum.Chasing, chase);
        patrol.AddTransition(EnemyStatesEnum.Hunting, hunt);
        ///attack
        attack.AddTransition(EnemyStatesEnum.Idle, idle);
        attack.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        attack.AddTransition(EnemyStatesEnum.Chasing, chase);
        attack.AddTransition(EnemyStatesEnum.Hunting, hunt);
        ///chase
        chase.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        chase.AddTransition(EnemyStatesEnum.Attacking, attack);
        chase.AddTransition(EnemyStatesEnum.Idle, idle);
        chase.AddTransition(EnemyStatesEnum.Hunting, hunt);
        ///hunt
        hunt.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        hunt.AddTransition(EnemyStatesEnum.Attacking, attack);
        hunt.AddTransition(EnemyStatesEnum.Idle, idle);
        hunt.AddTransition(EnemyStatesEnum.Chasing, chase);


        //[CustomEditor(typeof(NodeGrid))]




        var startNode = _model._startNode.transform.position;  ///asignar el start node a mano
        startNode.y = transform.localPosition.y;
        transform.position = startNode;
   

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
        /// esta el objetivo muerto?
        var isTargetDead = new TreeQuestion(IsTargetDie, idle, isInSight);

        _root = isTargetDead;

    }

    bool IsOnAttack()
    {
        ///El ataque va a depender de si puede atacar o el ataque esta activo y y si el rayo lo detecta
        ///si (/*puede atacar*/ |o| /* la duracion del ataque esta activa*/) y el rayo detecta?
        return ((_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform));
    }

    bool IsTargetDie()
    {
        return _target.IsDie;
    }
    bool IsOnHunt()
    {
        return _model.TargetSpotted;
    }
    bool IsInSight()
    {
        return _model.CheckRange(_target.transform) && _model.CheckAngle(_target.transform) && _model.CheckView(_target.transform);
    }
    void ActionIdle()
    {

        _fsm.Transitions(EnemyStatesEnum.Idle);
    }
    void ActionPatrol()
    {
        _fsm.Transitions(EnemyStatesEnum.Patrolling);
    }
    void ActionPursuit()
    {
        _fsm.Transitions(EnemyStatesEnum.Chasing);

    }
    void ActionHunt()
    {
        _fsm.Transitions(EnemyStatesEnum.Hunting);

    }
    void ActionAttack()
    {

        _fsm.Transitions(EnemyStatesEnum.Attacking);
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