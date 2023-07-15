using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoid
{
    Vector3 Velocity { get; }
    void LookDir(Vector3 dir);
    Vector3 Position { get; }
    Vector3 Front { get; }
    float Radius { get; }
}
