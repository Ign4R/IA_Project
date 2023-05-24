using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : ISteering
{
    PlayerModel _target;
    float _time;
    Transform _origin;

    public Pursuit(Transform origin, PlayerModel target, float time)
    {
        _origin = origin;
        _target = target;
        _time = time;
    }
    public virtual Vector3 GetDir()
    {
        float distance = Vector3.Distance(_origin.position, _target.transform.position);

        Vector3 point = _target.transform.position + _target.GetForward * Mathf.Clamp(_target.GetSpeed * _time, 0, distance - 3);
        return (point - _origin.position).normalized;
    }
}
