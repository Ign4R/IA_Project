using System.Collections;
using UnityEngine;
public class HunterState<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    SpiderModel _spiderModel;
    Transform _target;
    float _timerValue;
    Light _coneVision;

    public HunterState(ISteering obsAvoid, NodeGrid nodeGrid, Transform target, float timerState) : base(obsAvoid)
    {
        _astar = new AStar<Node>();
        _nodeGrid = nodeGrid;
        _target = target;
        _timerValue = timerState;

    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _spiderModel = (SpiderModel)model;
        _coneVision = _spiderModel._coneOfView;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.RunAnim;
        _coneVision.enabled = true;
        _coneVision.color = Color.magenta;
        CurrentTimer = _timerValue;
        Pathfinding();
        _model.LookDir(StartNode.transform.position + Avoid.GetDir().normalized);



    }

    public override void Execute()
    {

        base.Execute();
        Vector3 astarDir = Wp.GetDir() * _spiderModel._multiplierAstar;
        Vector3 avoidDir = Avoid.GetDir() * _spiderModel._multiplierAvoid;
        if (_endNode != null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);
            Vector3 posEnd = goalNodeFix - _model.transform.position;
            if (CurrentTimer > 0)
            {
                _spiderModel.CurrentTimerHunt = CurrentTimer;
                Timer();
                if (posEnd.magnitude < 0.2f)
                {
                    Pathfinding();
                    var newDir = Wp.GetDir() * _spiderModel._multiplierAstar;
                    astarDir = newDir;
                }
            }
            else if (posEnd.magnitude < 0.2f)
            {
                _spiderModel.SpottedTarget = false;

            }


        }


        Vector3 dirFinal = astarDir.normalized + avoidDir.normalized;
        _model.Move(dirFinal);
        _model.LookDir(dirFinal);
    }

    public void Pathfinding()
    {
        StartNode = _nodeGrid.GetNodeNearTarget(_model.transform);
        Node endNode = _nodeGrid.GetNodeNearTarget(_target, StartNode);
        _endNode = endNode;


        if (StartNode != null && _endNode != null)
        {
            var path = _astar.Run(StartNode, Satisfies, GetConnections, GetCost, Heuristic);
            if (path != null && path.Count > 0)
            {
                _spiderModel.GoalNode = _endNode;
                _spiderModel._startNode = StartNode;
                Wp.AddWaypoints(path);
            }
        }



    }

    public override void Sleep()
    {

        base.Sleep();
        _model.OnRun -= _view.RunAnim;
        StartNode = _endNode;
        _spiderModel._startNode = StartNode;
        _spiderModel.SpottedTarget = false;
        _coneVision.enabled = false;
    }
}
