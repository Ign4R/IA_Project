using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlocking 
{
    Vector3 GetDir(List<IBoid> boids, IBoid self);
}
