using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderView : BaseView
{
    public void AttackAnim(bool value)
    {       
        _anim.SetBool("Attack",value);
    }
}
