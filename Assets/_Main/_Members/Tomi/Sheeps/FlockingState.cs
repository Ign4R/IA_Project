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
        // Calculate the flocking direction
        Vector3 flockingDir = flockingManager.RunFlockingDir();
        
        // Update the movement and rotation of the sheep
        sheep.LookDir(flockingDir);
        sheep.Move(sheep.Front);
    }
}