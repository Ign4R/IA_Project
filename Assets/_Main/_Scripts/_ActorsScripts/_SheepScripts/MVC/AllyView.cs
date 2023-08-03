using System.Collections;
using UnityEngine;

public class AllyView : BaseView
{
    public Renderer[] renders;
    public Material material;
    public void AnimIdle(bool v)
    {
        _anim.SetBool("Idle", v);
        
    }

    public void ChangeColor(Color c)
    {
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material.color = c;
        }
    }
}
