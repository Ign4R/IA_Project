using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerView : BaseView
{

    public void OnTakeDamage(int lifes)
    {
        _anim.SetTrigger("Hit");
        //GameManager.Instance.UpdateLifes(lifes); 
    }

    public void OnDie()
    {
        _anim.SetBool("Dead", true);
    }
}



