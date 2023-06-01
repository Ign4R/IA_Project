using UnityEngine;

public class EnemyModel : BaseModel, IWaypoint
{
    public Transform[] waypoints;

    public Transform _originDamage;

    public bool _canAttack;

    public float _setMaxRandom;

    public float _setAttackTimer;

    public float _rangeDamage;

    public float _rangeView;

    public float _angleView;

    public LayerMask _mask;

    public LayerMask _targetMask;

    public PlayerModel _target=null;

    public int _iterations;

    Vector3 _posWP;

    int _currentPointIndex = 0;

    int _directionMultiplier = 1;

    public float CurrentTimerIdle { get; set; }
    public float CurrentTimerAttack { get; set; }
    public int IterationsInWp { get => _iterations; set => _iterations = value; }
    public float MaxValueRandom { get => _setMaxRandom; set => _setMaxRandom = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public bool AttackTimeActive { get ; set; }
    public float AttackTimer { get => _setAttackTimer; set => _setAttackTimer = value; }

    public void ApplyDamage()
    {

        Collider[] colliders = Physics.OverlapSphere(_originDamage.position, _rangeDamage, _targetMask);

        if (colliders.Length > 0)
        {
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
        transform.rotation = Quaternion.LookRotation(dir);
        transform.forward = dir;
    }
    public Vector3 GetDirWP()
    {
        _posWP = waypoints[_currentPointIndex].position;
        return (_posWP - transform.position).normalized;
    }
    public void TouchWayPoint()
    {
        if (Vector3.Distance(transform.position, _posWP) < 1)
        {
            _currentPointIndex += _directionMultiplier;

            if (_currentPointIndex >= waypoints.Length)
            {
                _currentPointIndex = waypoints.Length - 2;
                _directionMultiplier = -1;
            }
            else if (_currentPointIndex < 0)
            {
                _currentPointIndex = 1;
                _directionMultiplier = 1;
                IterationsInWp++;
            }
        }

    }

    ///Esta en el rango de vision
    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < _rangeView;
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

        return !Physics.Raycast(fixedOriginY, dirToTarget, out hit, distanceToTarget, _mask);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer== LayerMask.NameToLayer("Player"))
        {
            CanAttack = true;
            OnAttack(CanAttack);
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
        Gizmos.DrawWireSphere(transform.position, _rangeView);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleView / 2, 0) * transform.forward * _rangeView);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angleView / 2, 0) * transform.forward * _rangeView);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angleView / 2, 0) * transform.forward * _rangeView);
        Gizmos.color = Color.blue;
        Vector3 diff = _target.transform.position - transform.position;
        diff.y = 0;
        if (CheckRange(_target.transform) && CheckView(_target.transform) && CheckAngle(_target.transform))
            Gizmos.DrawRay(transform.position, diff);





    }


}
