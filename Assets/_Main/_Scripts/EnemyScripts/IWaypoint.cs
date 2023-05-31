using UnityEngine;

public interface IWaypoint
{
    int IterationsInWp { get; set; }
    void TouchWayPoint();
    Vector3 GetDirWP();
}
   

