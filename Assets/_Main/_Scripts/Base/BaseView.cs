using UnityEngine;
public class BaseView : MonoBehaviour
{
    public Animator _anim;
    public void RunAnim(Rigidbody rb)
    {
        float vel = rb.velocity.magnitude;
        _anim.SetFloat("Vel", vel);
    }

    public void IdleAnim(bool v)
    {
        _anim.SetBool("Idle", v);
    }

}

       

