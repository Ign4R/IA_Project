using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepFSM : MonoBehaviour
{
    private NewState currentState;

    public void ChangeState(NewState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}