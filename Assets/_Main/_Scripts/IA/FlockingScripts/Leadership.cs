using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leadership : MonoBehaviour, IFlocking
{
    public float _multiplierFollow;

    public Transform Target { get; private set; }
    public void SetLeader(Transform target)
    {
        Target = target;
    }
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 endDir= (Target.position - self.Position).normalized * _multiplierFollow;
        return endDir;
    }
}