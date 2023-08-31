using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCLeader_M : BaseModel, IWaypoint<Node>
{
    public Color leadColor;
    public LayerMask _maskTarget;
    public float _rotSpeed;
    public bool _addAlly;
    public List<GameObject> _allies = new List<GameObject>();
    public bool isVulnerable;
    public bool isTargetSpotted;

    public Node _startNode; //TODO
    public Node _goalNode;

    [Header("||--Obs Avoidance--||")]
    public LayerMask _maskAvoid;
    public int _maxObs;
    public float _radiusAvoid;
    public float _angleAvoid;

    [Header("||--Multiplier--||")]
    public float _multiplierAvoid;
    public float _multiplierPursuit;
    public float _multiplierAstar;

    [Header("||--Line Of View--||")]
    public float _radiusView;
    public float _angleView;
    public LayerMask _ignoreMask;
    public Light _coneOfView;

    [Header("||---Attack---||")]
    public Transform _originDamage; 
    public float _setAttackTimer;
    public float _rangeDamage;

    [Header("||--Ref Objetive--||")]
    public PlayerModel _target=null;

    private int _indexPoint = 0;
    List<Vector3> _waypoints = new List<Vector3>();

    public bool GoSafeZone { get; set; }
    public float CurrentTimerAttack { get; set; }
    public float CurrentTimerHunt { get; set; }
    public bool CanAttack { get; set; }
    public bool AttackTimeActive { get; set; }
    public Node GoalNode { get => _goalNode; set => _goalNode = value; }

    public Action OnAttack { get ; set ; }

    public override void Awake()
    {
     
        base.Awake();
    }

    public override void Move(Vector3 dir, float speedMulti = 1)
    {
        base.Move(dir, speedMulti);
    }
    public override void LookDir(Vector3 dir)
    {  
        if (dir == Vector3.zero) return;
        dir.y = 0;
        Vector3 dirAvoid = dir;
        transform.forward = Vector3.Lerp(transform.forward, dirAvoid, Time.deltaTime * _rotSpeed);

    }
    public void AddWaypoints(List<Node> points)
    {
        // print("SET WP?");
        _waypoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            _indexPoint = 0;
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

    public void CheckCollision()
    {
        float distance = (_target.transform.position - transform.position).sqrMagnitude;
        if (distance < 5)
        {
            Collider[] collisions = Physics.OverlapSphere(transform.position, 5);
            if (collisions.Length > 1)
            {

                //TODO acceder al componente de aliado y reclutarlo
            }
        }
    }
    public void Die(Transform spawn)
    {   
        transform.position = spawn.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            AllyModel sheep = other.GetComponent<AllyModel>();
            List<NPCLeader_M> leaders = sheep._leaders;
            if (sheep.ColorTeam != leadColor && sheep.InRisk) return;
            if (!leaders.Contains(this))
            {
                leaders.Add(this);
                if (sheep.HasLeader) return;
                sheep.HasLeader = true;
                sheep.ColorTeam = leadColor;
            }


        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusView);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleView / 2, 0) * transform.forward * _radiusView);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angleView / 2, 0) * transform.forward * _radiusView);
        Gizmos.color = Color.blue;

    }

    

}

public static class Extensions
{
    public static bool ContainsLayer(this LayerMask layerMask ,int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}