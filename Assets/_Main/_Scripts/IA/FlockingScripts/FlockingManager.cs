
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public bool _hasLeader;
    public Transform _theirLeader;
    public int maxBoids = 5;
    public LayerMask maskBoids;
    IBoid _self;
    IFlocking[] _flockings;
    Leadership _leader;
    Collider[] _colliders;
    List<IBoid> _boids;

    public float Distance { get; private set; }
    public List<IBoid> Boids { get => _boids; set => _boids = value; }
    public Transform HisLeader { get => _theirLeader; set => _theirLeader = value; }

    private void Awake()
    {
        _self = GetComponent<IBoid>();
        _flockings = GetComponents<IFlocking>();
        _boids = new List<IBoid>();
        _colliders = new Collider[maxBoids];
     
    }

    public void GetFlockLeader(Transform target)
    {
        for (int i = 0; i < _flockings.Length; i++)
        {
            if (_flockings[i] is Leadership leader)
            {
                leader.SetLeader(target);
                break;
            }
        }
    }
    public void Clear()
    {
        _colliders=new Collider[0];
    }
    public Vector3 RunFlockingDir()
    {
        _boids.Clear();
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

        for (int i = 0; i < _flockings.Length; i++)
        {
            IFlocking currFlock = _flockings[i];
            dir += currFlock.GetDir(_boids, _self);

        }
        return dir.normalized;
    }
}