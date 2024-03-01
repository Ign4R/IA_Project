using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public float _multiplierFLK;
    public bool _hasLeader;
    public Transform _theirLeader;
    public int _maxBoids = 5;
    public LayerMask maskBoids;
    public Leadership _leadership;
    IBoid _self;
    IFlocking[] _flockings;
    Collider[] _colliders;
    List<IBoid> _boids;
    private int _numBoids;
    public Transform _target;

    public float Distance { get; private set; }
    public List<IBoid> Boids { get => _boids; set => _boids = value; }
    public Transform HisLeader { get => _theirLeader; set => _theirLeader = value; }
    public Collider[] Colliders { get => _colliders; set => _colliders = value; }

    private void Awake()
    {
        _self = GetComponent<IBoid>();
        _flockings = GetComponents<IFlocking>();
        _boids = new List<IBoid>();
        _colliders = new Collider[_maxBoids];

       
    }

    public void SetFlockLeader(Transform target)
    {
        _target = target;
        _leadership.SetLeader(target);
    }
    public void ResetFlockLeader()
    {      
        _leadership.SetLeader(null);
    }
    public void Clear()
    {
        _colliders=new Collider[0];
    }
    public Vector3 RunFlockingDir()
    {

        _boids.Clear();
        if (_target == null) return Vector3.zero;
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
        for (int i = 0; i < _flockings.Length; i++)
        {

            var currFlock = _flockings[i];
            dir += currFlock.GetDir(_boids, _self);
        }
        return dir.normalized;
    }


    public int CountCollision()
    {
        return Physics.OverlapSphereNonAlloc(_self.Position, _self.Radius, _colliders, maskBoids);
    }
}