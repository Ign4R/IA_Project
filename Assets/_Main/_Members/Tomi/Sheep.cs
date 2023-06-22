using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IBoid, IFlocking
{
   //VARIABLES DE VELOCIDAD, ROTACIÓN Y RB DEL BOID:
   public float speed;
   public float rotSpeed;
   public float radius;
   private Rigidbody rb;
   
   //FLOCKING:
   public Cohesion cohesion;
   public Avoidance avoidance;
   public Alignment alignment;
   
   //FLOCKING ADICIONALES:
   public Leader leader;
   public Predator predator;

   //INICIALIZA EL RB DEL BOID
   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }

   //INTERFACES IBOID, IFLOCKING
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
      
      //POR SI LA ROTACIÓN DE LAS OVEJAS NO FUNCIONA BIEN AL MOVERSE:
      // //  dir.y = 0;
      // transform.forward = Vector3.Lerp(transform.forward, -dir, rotSpeed * Time.deltaTime);
   }

   //VISUALIZACION DEL PERSONAL RADIUS
   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(Position, radius);
   }

   //Método que ejecuta los comportamientos de flocking  asignados desde Inspector:
   public Vector3 GetDir(List<IBoid> boids, IBoid self)
   {
      Vector3 dir = Vector3.zero;

      // Cohesion behavior
      dir += cohesion.GetDir(boids, self);

      // Avoidance behavior
      dir += avoidance.GetDir(boids, self);

      // Alignment behavior
      dir += alignment.GetDir(boids, self);

      // Leader behavior
      dir += leader.GetDir(boids, self);

      // Predator behavior
      dir += predator.GetDir(boids, self);

      return dir.normalized;
   }

}