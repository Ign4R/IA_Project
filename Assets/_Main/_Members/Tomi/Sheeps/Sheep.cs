using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IBoid
{
   // VARIABLES DE VELOCIDAD, ROTACIÓN Y RB DEL BOID:
   public float speed;
   public float rotSpeed;
   public float radius;
   private Rigidbody rb;
   
   // INICIALIZA EL RB DEL BOID
   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }
   

   // INTERFACES IBOID
   public Vector3 Position => transform.position;

   public Vector3 Front => transform.forward;

   public float Radius => radius;

   public void Move(Vector3 dir)
   {
      dir *= speed;
      dir.y = rb.velocity.y;
      rb.velocity = dir;
   }

   public void LookDir(Vector3 dir)
   {
      dir.y = 0;
      transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
   }

   // VISUALIZACION DEL PERSONAL RADIUS
   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(Position, radius);
   }
}