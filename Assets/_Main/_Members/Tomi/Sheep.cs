using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
   //Lógica de movimiento y llamado de comportamiento de Flocking de las ovejas( similar al Batman script de la clase de Sebas)
   //Amoldar según movimiento de las mismas en la escena.
   
   public float speed;
   public float rotSpeed;
   public float radius;
   Rigidbody _rb;
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
  
    
    
}