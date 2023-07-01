using UnityEngine;
public class BaseView : MonoBehaviour
{
    public Animator _anim;
    public void AnimRun(Rigidbody rb)
    {
        float vel = rb.velocity.magnitude;
        _anim.SetFloat("Vel", vel);

        ///if (this is EnemyView) print(" Anim Run Enemy " + gameObject.transform.parent.name);

    }

}

       

