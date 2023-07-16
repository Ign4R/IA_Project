using System.Collections;
using UnityEngine;

public class AllyView : BaseView
{
    public void AnimIdle(bool v)
    {
        _anim.SetBool("Idle", v);
        
    }
}
