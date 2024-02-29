using System;
using UnityEngine;

public class TreeAction: ITreeNode
{
    public Action OnAction { get; private set; }
    public Enum Name { get; private set; }
    public TreeAction(Action action, Enum name = default)
    {
        OnAction = action;
        Name = name;
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
