
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public PlayerModel _target;  //*TODO
    public int maxBoids = 5;
    public LayerMask maskBoids;
    IBoid _self;
    IFlocking[] _flockings;
    Collider[] _colliders;
    List<IBoid> _boids;

    public float Distance { get; private set; }
    public List<IBoid> Boids { get => _boids; set => _boids = value; }

    private void Awake()
    {
        _self = GetComponent<IBoid>();
        _flockings = GetComponents<IFlocking>();
        _boids = new List<IBoid>();
        _colliders = new Collider[maxBoids];
    }
    public void Clear()
    {
        _colliders=new Collider[0];
    }
    public Vector3 RunFlockingDir()
    {

        _boids.Clear();

        var distance = Vector3.Distance(_self.Position, _target.transform.position);
        Distance = distance;
        //Physics.OverlapSphere(_self.Position, _self.Radius);
        int count = Physics.OverlapSphereNonAlloc(_self.Position, _self.Radius, _colliders, maskBoids);
        Vector3 dir = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            var curr = _colliders[i];
            IBoid boid = curr.GetComponent<IBoid>();

            if (boid == null) continue;
            _boids.Add(boid);
        }
        if (Distance <= 5)
        {

            Vector3 avoidDir = Vector3.zero;
            foreach (var flocking in _flockings)
            {
                if (_self != flocking)
                {
                    avoidDir += flocking.GetDir(_boids, _self);
                }
              
            }
            avoidDir /= (_flockings.Length);


            Vector3 avoidCollisionDir = Vector3.zero;
            foreach (var boid in _boids)
            {
                if (boid != _self)
                {
                    Vector3 separationDir = (_self.Position - boid.Position).normalized;
                    avoidCollisionDir += separationDir;
                }

            }
            avoidCollisionDir /= (_boids.Count - 1); // Restar 1 para excluir a _self de la cuenta


            Vector3 finalDir = -avoidDir + (4* avoidCollisionDir);
            return finalDir.normalized;

        }
        else
        {
            for (int i = 0; i < _flockings.Length; i++)
            {

                var currFlock = _flockings[i];
                dir += currFlock.GetDir(_boids, _self);
            }
        }
        return dir.normalized;
    }
}