using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : BaseView
{
    public void OnTakeDamage()
    {
        _anim.SetTrigger("Hit");
    }
}
