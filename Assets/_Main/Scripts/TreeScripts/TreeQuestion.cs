using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreeQuestion : ITreeNode
{
    Func<bool> _question;
    ITreeNode _tNode;
    ITreeNode _fNode;
    public TreeQuestion(Func<bool> question,ITreeNode Tnode, ITreeNode Fnode)
    {
        _question = question;
        _tNode = Tnode;
        _fNode = Fnode;

    }
    public void Execute()
    {
        if(_question())
        {
            _tNode.Execute();
        }
        else
        {
            _fNode.Execute();
        }
    }
}
