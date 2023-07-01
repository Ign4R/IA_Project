using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 front = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            front += boids[i].Front;
        }
        return front.normalized * multiplier;
    }
}
