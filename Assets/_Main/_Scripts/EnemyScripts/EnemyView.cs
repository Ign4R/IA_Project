using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : BaseView
{
    public void AnimAttack(bool v)
    {       
        _anim.SetBool("Attack", v);
    }
}
