using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerView : BaseView
{

    public void OnTakeDamage()
    {
        _anim.SetTrigger("Hit");
    }

    public void OnDie()
    {
        _anim.SetBool("Dead", true);
    }
}



