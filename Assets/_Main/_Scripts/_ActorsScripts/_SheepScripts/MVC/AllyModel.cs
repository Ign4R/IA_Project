using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyModel : BaseModel, IBoid
{
    
    public int _escapeW;
    public int _affinityW;
    public int _dieW;

    public float _alliesNear;
    public LayerMask _avoidMask;
    public int _maxObs;
    public float _angleAvoid;
    public float speed;
    public float _rotSpeed;
    public float _radius;

    public ISteering ObsAvoid { get; set; }
    public Action<bool> OnIdle;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
    public float Radius => _radius;
    public bool IsStop { get; set; } //*TODO
    public SpriteRenderer Icon { get => _icon; set => _icon = value; }

    public Vector3 Velocity =>_rb.velocity;

    public List<NPCLeader_M> _leaders = new List<NPCLeader_M>();

    private SpriteRenderer _icon;
    public Color ColorTeam { get; set; }

    public Vector3 AvoidDir => ObsAvoid.GetDir();

    public bool HasLeader;
    public bool InRisk;
    public float _multiplierAvoid;


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


    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Position, _radius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleAvoid / 2, 0) * transform.forward * _radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angleAvoid / 2, 0) * transform.forward * _radius);


    }

}


