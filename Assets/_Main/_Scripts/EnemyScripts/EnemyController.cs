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

    void InitializedFSM()
    {
        _fsm = new FSM<EnemyStatesEnum>();

        ISteering pursuit = new Pursuit(_model.transform, _target, _timePredict); ///*PRIORITY:BAJA-(crearlo en otro lado)
        var chase = new EnemyStateChase<EnemyStatesEnum>(pursuit);

        var idle = new EnemyStateIdle<EnemyStatesEnum>(EnemyStatesEnum.Patrolling);

        var patrol = new EnemyStatePatrol<EnemyStatesEnum>(_nodeGrid);

        var attack = new EnemyStateAttack<EnemyStatesEnum>();

        idle.InitializedState(_model,_view, _fsm);
        patrol.InitializedState(_model, _view, _fsm);
        attack.InitializedState(_model, _view, _fsm);
        chase.InitializedState(_model, _view, _fsm);

        idle.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        idle.AddTransition(EnemyStatesEnum.Attack, attack);
        idle.AddTransition(EnemyStatesEnum.Chasing, chase);
        patrol.AddTransition(EnemyStatesEnum.Idle, idle);
        patrol.AddTransition(EnemyStatesEnum.Attack, attack);
        patrol.AddTransition(EnemyStatesEnum.Chasing, chase);
        attack.AddTransition(EnemyStatesEnum.Idle, idle);
        attack.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        attack.AddTransition(EnemyStatesEnum.Chasing, chase);
        chase.AddTransition(EnemyStatesEnum.Patrolling, patrol);
        chase.AddTransition(EnemyStatesEnum.Attack, attack);


        attack.SetTimer(_model.AttackTimer);
        idle.SetTimer(_model.MaxValueRandom);
        _fsm.SetInit(patrol);
    }

    public void InitializeTree()
    {
        ///actions
        var idle = new TreeAction(ActionIdle);
        var patrol = new TreeAction(ActionPatrol);
        var attack = new TreeAction(ActionAttack);
        var chase = new TreeAction(ActionPursuit); 

        ///questions
  
        ///mate al player?
        var isIterOver = new TreeQuestion(IsterOver, idle, chase);
        /// esta el pj en mi rango de vision?
        var isInSight = new TreeQuestion(IsInSight, isIterOver, patrol);
        /// el ataque ha terminado?
        var isOnAttack = new TreeQuestion(IsOnAttack, attack, isInSight);
        _root = isOnAttack;
            
    }

    bool IsOnAttack()
    {
        ///El ataque va a depender de si puede atacar o el ataque esta activo y y si el rayo lo detecta
        ///si (/*puede atacar*/ |o| /* la duracion del ataque esta activa*/) y el rayo detecta?
        return (_model.CanAttack || _model.AttackTimeActive) && _model.CheckView(_target.transform) && !_target.IsDie;
    }

    bool IsterOver()
    {
        return _model.IterationsInWp >= 5|| _target.IsDie;
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
    void ActionAttack()
    {
        
        _fsm.Transitions(EnemyStatesEnum.Attack);
    }

    private void Update()
    {

        _fsm.OnUpdate();
        _root.Execute();
    }
}
