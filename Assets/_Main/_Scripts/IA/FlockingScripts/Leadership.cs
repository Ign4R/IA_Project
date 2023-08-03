using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leadership : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Transform Target { get; private set; }
    public void SetLeader(Transform target)
    {
        Target = target;
    }
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        return (Target.position - self.Position).normalized * multiplier;
    }
}