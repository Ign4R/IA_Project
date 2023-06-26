using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingState : NewState
{
    private FlockingManager flockingManager;
    private Sheep sheep;
    private void Start()
    {
        sheep = GetComponent<Sheep>();
        flockingManager = GetComponent<FlockingManager>();
    }


    public override void Update()
    {
        base.Update();
        // Calculate the flocking direction
        if (!sheep.IsStop)
        {
            Vector3 flockingDir = flockingManager.RunFlockingDir();

            // Update the movement and rotation of the sheep
            sheep.LookDir(flockingDir);
            sheep.Move(sheep.Front);
        }
        else
        {
            sheep.LookDir(Vector3.forward);
            sheep.Move(Vector3.zero);
        }

    }

}