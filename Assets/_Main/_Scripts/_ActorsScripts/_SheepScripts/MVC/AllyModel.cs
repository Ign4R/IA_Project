using System;
using UnityEditor;
using UnityEngine;

public class AllyModel : BaseModel, IBoid
{
    public LayerMask _avoidMask;
    public int _maxObs;
    public float _avoidAngle;
    public float _avoidRange;
    public float _childSize;
    public bool targetIsDead;
    public float speed;
    public float _rotSpeed;
    public float radius;
    public Action<bool> OnIdle;
    public Transform HisLeader { get; private set; }
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
    public float Radius => radius;
    public bool IsStop { get; set; } //*TODO
    public SpriteRenderer Icon { get => _icon; set => _icon = value; }

    public Vector3 Velocity =>_rb.velocity;

    private Transform _parent;
    private SpriteRenderer _icon;
   

    private void Start()
    {

        Icon = GetComponentInChildren<SpriteRenderer>();
        _parent = transform.parent.GetComponent<Transform>();

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

    public void ProcreateSheep()
    {
        var child = Instantiate(gameObject, transform.position + Vector3.right, transform.rotation, _parent);
        child.transform.localScale = new Vector3(_childSize, _childSize, _childSize);

    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Position, radius);
    }

}


