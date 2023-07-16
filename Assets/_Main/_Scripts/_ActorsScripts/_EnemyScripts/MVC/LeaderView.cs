using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderView : BaseView
{
    public void AnimAttack(bool v)
    {       
        _anim.SetBool("Attack", v);
    }
}
