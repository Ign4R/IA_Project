using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IBoid
{
   // VARIABLES DE VELOCIDAD, ROTACIÓN Y RB DEL BOID:
   public float speed;
   public float rotSpeed;
   public float radius;
   private bool isStop;
   private Rigidbody rb;
   private Animator _animator;
   
   // INICIALIZA EL RB DEL BOID
   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
      _animator = GetComponent<Animator>();
   }
   
   // INTERFACES IBOID
   public Vector3 Position => transform.position;

   public Vector3 Front => transform.forward;

   public float Radius => radius;

    public bool IsStop { get => isStop; set => isStop = value; }

    public void Move(Vector3 dir)
   {
      dir *= speed;
      dir.y = rb.velocity.y;
      rb.velocity = dir;
   }
   
   public void StopMovement()
   {
        isStop = true;
        _animator.SetBool("Idle", true);
      // Desactiva el movimiento de las ovejas ( para la SafeZone.)
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