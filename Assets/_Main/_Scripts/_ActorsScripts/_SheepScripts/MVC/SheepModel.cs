using System;
using UnityEditor;
using UnityEngine;

public class SheepModel : BaseModel, IBoid
{

    public bool targetIsDead;
    public float speed;
    public float _rotSpeed;
    public float radius;
    public Action<bool> OnIdle;

    public Vector3 Position => transform.position;

   public Vector3 Front => transform.forward;

   public float Radius => radius;

   public bool IsStop { get;  set; } //*TODO
  

   public override void LookDir(Vector3 dir)
   {
       
        if (dir == Vector3.zero) return;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _rotSpeed);

   }

    private void Update()
    {
  
    }
    private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(Position, radius);
   }
}


