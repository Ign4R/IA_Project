using UnityEngine;
using System.Collections.Generic;

public interface IWaypoint<T>
{
    void AddWaypoints(List<T> points);
    Vector3 GetDir();
}
   

