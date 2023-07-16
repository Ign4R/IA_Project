using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLeader_V : BaseView
{
    public void AnimAttack(bool v)
    {       
        _anim.SetBool("Attack", v);
    }
}
