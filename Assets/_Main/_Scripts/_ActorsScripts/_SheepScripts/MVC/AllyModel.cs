using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyModel : BaseModel, IBoid
{
    [SerializeField] private Color colorFollow;
    /// Stats Dynamic
    public bool HasLeader;
    public bool isScared;
    [SerializeField]
    [ReadOnly]
    public float _leaveW;
    [SerializeField]
    [ReadOnly]
    public float _affinityW;
    [SerializeField]
    [ReadOnly]
    public float _dieW;
    [SerializeField]
    [ReadOnly]
    public float _alliesNear;

    /// Multipliers
    [SerializeField] private float _multiplyLeave;
    [SerializeField] private float _multiplyAffinity;
    [SerializeField] private float _multiplyDie=0;


   //Avoid
    public LayerMask _avoidMask;
    public int _maxObs;
    public float _angleAvoid;
    public float _rotSpeed;
    public float _radius;
    public float _multiplierAvoid;

    public LayerMask maskEnemies;
    public LayerMask _maskPlayer;
    public Collider[] CollsEnemies { get; set; } = new Collider[5];
    public ISteering ObsAvoid { get; set; }
    public Action<bool> OnIdle;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
    public float Radius => _radius;
    public bool IsStop { get; set; } //*TODO
    public SpriteRenderer Icon { get => _icon; set => _icon = value; }

    public Vector3 Velocity =>_rb.velocity;

    public List<Transform> _leaders = new List<Transform>();

    private SpriteRenderer _icon;

    public Vector3 AvoidDir => ObsAvoid.GetDir();

    public float MultiplyLeave { get => _multiplyLeave; set => _multiplyLeave = value; }
    public float MultiplyAffinity { get => _multiplyAffinity; set => _multiplyAffinity = value; }
    public float MultiplyDie { get => _multiplyDie; set => _multiplyDie = value; }
    public Color ColorFollow { get => colorFollow; set => colorFollow = value; }

    private void Start()
    {
        Icon = GetComponentInChildren<SpriteRenderer>();  

    }


    public override void Move(Vector3 dir, float speedMulti = 1)
    {
        base.Move(dir, speedMulti);
    }
    public override void LookDir(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _rotSpeed);
    }

    public void Die()
    {
        OnDie();
    }
    
    public void Freeze()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Position, _radius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleAvoid / 2, 0) * transform.forward * _radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angleAvoid / 2, 0) * transform.forward * _radius);


    }

}


