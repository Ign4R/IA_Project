using UnityEngine;
public class PlayerController : BaseController
{
    public PlayerModel _model;
    public PlayerView _view;
    FSM<PlayerStatesEnum> _fsm;

    private void Awake()
    {
        _model.OnTakeDamage += _view.OnTakeDamage;
        InitializedFSM();
    }

    private void Update()
    {     
        _fsm.OnUpdate();
    }
    public void InitializedFSM()
    {
        _fsm = new FSM<PlayerStatesEnum>();
        var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Movement);
        var move = new PlayerStateMove<PlayerStatesEnum>(PlayerStatesEnum.Idle);
        var hurt = new PlayerStateHurting<PlayerStatesEnum>(PlayerStatesEnum.Idle);

        
        idle.AddTransition(PlayerStatesEnum.Movement, move);
        idle.AddTransition(PlayerStatesEnum.Hurting, hurt);
        move.AddTransition(PlayerStatesEnum.Idle, idle);
        move.AddTransition(PlayerStatesEnum.Hurting, hurt);

        hurt.AddTransition(PlayerStatesEnum.Movement, move);
        hurt.AddTransition(PlayerStatesEnum.Idle, idle);

        idle.InitializedState(_model, _view, _fsm);
        move.InitializedState(_model, _view, _fsm);
        hurt.InitializedState(_model, _view, _fsm);

      

        _fsm.SetInit(idle);
    }
    public void ActionHurting(bool v)
    {
        //_view.OnTakeDamage(v);
        //if (v) _fsm.Transitions(PlayerStatesEnum.Hurting);
        //else _fsm.Transitions(PlayerStatesEnum.Idle);

    }


}
