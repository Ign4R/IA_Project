using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseModel : MonoBehaviour
{
    protected Rigidbody _rb;
    private float _speed = 4;
    public Action<Rigidbody> OnRun;
    public Action OnDie;
    protected bool isDie;

    public ObstacleAvoidance ObsAvoid { get; private set; }
    public Vector3 GetForward => transform.forward;
    public float GetSpeed => _rb.velocity.magnitude;
    public Rigidbody Rb { get => _rb; }


    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public virtual void Move(Vector3 dir, float multiplier = 1)
    {
        dir.y = 0;
        Vector3 dirSpeed = dir * (_speed * multiplier);
        dirSpeed.y = _rb.velocity.y;
        _rb.velocity = dirSpeed;
        if (OnRun != null) OnRun(_rb);

    }

    public virtual void LookDir(Vector3 dir)
    {

    }



}