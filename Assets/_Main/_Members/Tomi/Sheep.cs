using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IBoid, IFlocking
{
   public float speed;
   public float rotSpeed;
   public float radius;
   private Rigidbody _rb;

   private void Awake()
   {
      _rb = GetComponent<Rigidbody>();
   }

   public Vector3 Position => transform.position;

   public Vector3 Front => transform.forward;

   public float Radius => radius;

   public void Move(Vector3 dir)
   {
      dir *= speed;
      dir.y = _rb.velocity.y;
      _rb.velocity = dir;
   }

   public void LookDir(Vector3 dir)
   {
      dir.y = 0;
      transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(Position, radius);
   }

   public Vector3 GetDir(List<IBoid> boids, IBoid self)
   {
      // Implementa el comportamiento de flocking específico de las ovejas aquí
      // y devuelve la dirección resultante.
      // Puedes usar la lista de boids cercanos y los parámetros del self para calcular la dirección.

      // Ejemplo de comportamiento: mantenerse cerca de los boids vecinos
      Vector3 dir = Vector3.zero;
      foreach (var boid in boids)
      {
         Vector3 toBoid = boid.Position - self.Position;
         dir += toBoid;
      }
      return dir.normalized;
   }
}