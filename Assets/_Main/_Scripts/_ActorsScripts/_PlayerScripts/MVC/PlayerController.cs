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
    private void Start()
    {
        _model.OnDie += _view.OnDie;
        _model.OnDie += ActionDie;
    }
    private void Update()
    {         
        _fsm.OnUpdate();
    }
    public void InitializedFSM()
    {
        _fsm = new FSM<PlayerStatesEnum>();
        var idle = new PlayerIdleState<PlayerStatesEnum>(PlayerStatesEnum.Movement);
        var move = new PlayerMoveState<PlayerStatesEnum>(PlayerStatesEnum.Idle);
        var die = new PlayerDieState<PlayerStatesEnum>();

        
        idle.AddTransition(PlayerStatesEnum.Movement, move);
        idle.AddTransition(PlayerStatesEnum.Die, die);
        move.AddTransition(PlayerStatesEnum.Idle, idle);
        move.AddTransition(PlayerStatesEnum.Die, die);

        die.AddTransition(PlayerStatesEnum.Movement, move);
        die.AddTransition(PlayerStatesEnum.Idle, idle);

        idle.InitializedState(_model, _view, _fsm);
        move.InitializedState(_model, _view, _fsm);
        die.InitializedState(_model, _view, _fsm);

      

        _fsm.SetInit(idle);
    }
    public void ActionDie()
    {
        _fsm.Transitions(PlayerStatesEnum.Die);

    }


}
