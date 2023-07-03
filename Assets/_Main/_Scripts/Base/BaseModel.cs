using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseModel : MonoBehaviour
{
    protected Rigidbody _rb;
    public float _speed = 4;
    public Action<Rigidbody> OnRun;
    public Action OnDie;
    protected bool isDie;

    public Vector3 GetForward => transform.forward;
    public float GetSpeed => _rb.velocity.magnitude;

    public bool IsDie { get => isDie; protected set => isDie = value; }

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public virtual void Move(Vector3 dir)
    {
        dir.y = 0;
        Vector3 dirSpeed = dir * _speed;
        dirSpeed.y = _rb.velocity.y;
        _rb.velocity = dirSpeed;
        if (OnRun != null) OnRun(_rb);
    }


    public virtual void LookDir(Vector3 dir)
    {

    }



}