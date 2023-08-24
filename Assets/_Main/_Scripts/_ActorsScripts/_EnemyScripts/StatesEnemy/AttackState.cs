using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState<T> : NavigationState<T>
{ 
    NPCLeader_M _enemyM;
    NPCLeader_V _enemyV;
    Transform _target;
    Transform _spawn;

    public AttackState(Transform target,Transform spawn) : base(null)
    {
        _target = target;
        _spawn = spawn;
    }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyM = (NPCLeader_M)model;
        _enemyV = (NPCLeader_V)view;

    }
    public override void Awake()
    {
        base.Awake();
        _model.Move(Vector3.zero);
        _enemyV.AnimAttack(true);

    }

    public override void Execute()
    {
        CheckCollision();
    }
    public void CheckCollision()
    {
        float distance = (_target.transform.position - _model.transform.position).sqrMagnitude;
        if (distance < 15)
        {
            NPCLeader_M leader = _target.GetComponent<NPCLeader_M>();
            leader.Die(_spawn);
            leader.isTargetSpotted = false;
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        _enemyV.AnimAttack(false); ///TODO al salir rapidamente de este estado la animacion de ataque no se logra apreciar

    }


}
