using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Transform target;
    public float followDistance = 5f; // Distancia de seguimiento de PLAYER
    public float randomDirectionInterval = 1f; // Intervalo de tiempo para cambiar a dirección RANDOM

    private Vector3 _randomDirection;
    private float _randomDirectionTimer;

    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        float distance = Vector3.Distance(self.Position, target.position);
        if (distance <= followDistance)
        {
            return (target.position - self.Position).normalized * multiplier;
        }
        else
        {
            // Return a consistent random direction for a certain amount of time
            _randomDirectionTimer -= Time.deltaTime;
            if (_randomDirectionTimer <= 0)
            {
                _randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                _randomDirectionTimer = randomDirectionInterval;
            }

            return _randomDirection * multiplier;
        }
    }
}