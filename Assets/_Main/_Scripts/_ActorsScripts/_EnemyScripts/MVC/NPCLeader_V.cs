using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLeader_V : BaseView
{
    public void AttackAnim()
    {       
        _anim.SetTrigger("Attack");
    }
}
