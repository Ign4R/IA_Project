using System;
using UnityEngine;

public class TreeAction : ITreeNode
{
    public Action OnAction { get; private set; }
    public TreeAction(Action action)
    {
        OnAction = action;
    }
    public void Execute()
    {
        if(OnAction!=null)
        OnAction();
        else
        {

        }

    }
}
