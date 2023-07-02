using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : BaseModel, IWaypoint<Node>
{
    public Node _startNode; //TODO

    [Header("||--Multiplier--||")]
    [Space(10)]
    public float _rotSpeed;
    public float _multiplierAvoid;
    public float _multiplierPursuit;
    public float _multiplierAstar;

    [Header("||--Line Of View--||")]
    [Space(10)]
    public float _radiusView;
    public float _angleView;
    public LayerMask _ignoreMask;
    public GameObject _coneOfView;

    [Header("||--Obs Avoidance--||")]
    [Space(10)]
    public LayerMask _maskAvoid;
    public int _maxObs;
    public float _radiusAvoid;
    public float _angleAvoid;

    [Header("||---Attack---||")]
    [Space(10)]
    public Transform _originDamage; 
    public float _setMaxRandom;
    public float _setAttackTimer;
    public float _rangeDamage;
    public LayerMask _maskTarget;

    [Header("||--Ref Player--||")]
    [Space(10)]
    public PlayerModel _target=null;

    ///Private
    bool _canAttack;
    int _indexPoint = 0;
    List<Vector3> _waypoints = new List<Vector3>();

    public float CurrentTimerIdle { get; set; }
    public float CurrentTimerAttack { get; set; }
    public float MaxValueRandom { get => _setMaxRandom; set => _setMaxRandom = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public bool AttackTimeActive { get ; set; }
    public float AttackTimer { get => _setAttackTimer; set => _setAttackTimer = value; }
    public Node GoalNode { get ; set; }
    public ISteering ObsAvoidance { get ; set ; }
    public Action<bool> OnAttack { get ; set ; }

    public override void Awake()
    {
        base.Awake();
        ObsAvoidance = new ObstacleAvoidance(transform, _maskAvoid, _maxObs, _angleAvoid, _radiusAvoid);
    }  
    public void ApplyDamage()
    {
        var target = LayerMask.NameToLayer("Player");

        Collider[] colliders = Physics.OverlapSphere(_originDamage.position, _rangeDamage, _maskTarget);

        if (colliders.Length > 0)
        {
            print("entre");
            PlayerModel player = colliders[0].GetComponent<PlayerModel>();

            if (player != null)
            {
                print("Take Life Player");
                player.TakeLife();
            }
        }

    }
    public override void LookDir(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _rotSpeed);

    }
    public void AddWaypoints(List<Node> points)
    {
       // print("SET WP?");
        _waypoints.Clear();
        for (int i = 0; i < points.Count; i++)
        { 
            _waypoints.Add(points[i].transform.position);
        }

    }
    public Vector3 GetDir()
    {
      
 
        if (_waypoints != null && _waypoints.Count > 0 && _indexPoint < _waypoints.Count)
        {

            Vector3 point = _waypoints[_indexPoint];
            Vector3 nextPoint = new Vector3(point.x, transform.position.y, point.z);
            Vector3 dir = nextPoint - transform.position;
            if (dir.magnitude < 0.2f)
            {
                _indexPoint++;
                if (_indexPoint >= _waypoints.Count)
                {
                    _indexPoint = 0;
                }
            }
            dir = _waypoints[_indexPoint] - transform.position;

            return dir.normalized;
        }
        else
        {
            _indexPoint = 0;
            Debug.Log("WAYPOINTS IS NULL OR IS COUNT ZERO  (CHECK NODES NEIGHS)");
        }

        return Vector3.zero;


    }

    ///Esta en el rango de vision
    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < _radiusView;
    }

    ///Esta en el angulo de vision
    public bool CheckAngle(Transform target) ///TODO: SIEMPRE PASARLE EL TRANSFORM DEL PLAYER MODEL
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(forward, dirToTarget);
        return _angleView / 2 > angleToTarget;
    }
    ///Esta en el rayo de vision, y lo esta "hiteando" al target primero
    public bool CheckView(Transform target)
    {
        Vector3 diff = target.position - transform.position;
        float distanceToTarget = diff.magnitude;
        Vector3 dirToTarget = diff.normalized;
        Vector3 fixedOriginY = transform.position ;

        RaycastHit hit;

        return !Physics.Raycast(fixedOriginY, dirToTarget, out hit, distanceToTarget, _ignoreMask);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer== LayerMask.NameToLayer("Player"))
        {
            CanAttack = true;
            OnAttack(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CanAttack = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_originDamage.position, _rangeDamage);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusView);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleView / 2, 0) * transform.forward * _radiusView);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angleView / 2, 0) * transform.forward * _radiusView);

        Gizmos.color = Color.blue;
        Vector3 diff = _target.transform.position - transform.position;
        diff.y = 0;
        if (CheckRange(_target.transform) && CheckView(_target.transform) && CheckAngle(_target.transform))
            Gizmos.DrawRay(transform.position, diff);





    }

    
  
}