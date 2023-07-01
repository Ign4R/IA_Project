using System.Collections;
using UnityEngine;

public class SheepView : BaseView
{
    public void AnimIdle(bool v)
    {
        _anim.SetBool("Idle", v);
        
    }
}
