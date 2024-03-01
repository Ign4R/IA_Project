using System;
using System.Collections;
using UnityEngine;
public class HunterState<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    SpiderModel _spiderM;
    float _timerValue;
    Transform _target;
    Vector3 newAstarDir;

    public HunterState(ISteering obsAvoid, Transform target,float timerState): base(obsAvoid)
    {
        _astar = new AStar<Node>();
        _target = target;
        _timerValue = timerState;

    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _spiderM = (SpiderModel)model;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.RunAnim;
        _spiderM._coneOfView.color = Color.magenta;
        CurrentTimer = _timerValue;
        _model.Move(Vector3.zero);
        Pathfinding();
        //var startNode= _nodeGrid.FindNearestValidNode(_model.transform);
        //_model.LookDir(startNode.transform.position);


    }

    public override void Execute()
    {
        base.Execute();
        newAstarDir = Wp.GetDir() * _spiderM._multiplierAstar;
        Vector3 avoidDir = Avoid.GetDir() * _spiderM._multiplierAvoid;
        TimeForHunt();
        Vector3 dirFinal = newAstarDir.normalized + avoidDir.normalized;
        _model.Move(dirFinal);
        _model.LookDir(dirFinal * 2);
    }
    public void TimeForHunt()
    {
        var posEnd = _endNode.transform.position;
        if (CurrentTimer > 0)
        {
            _spiderM.CurrentTimerHunt = CurrentTimer;
            Timer();
            if (posEnd.magnitude < 0.2f)
            {
                Pathfinding();
                newAstarDir = Wp.GetDir() * _spiderM._multiplierAstar;
            }
        }
        else if (posEnd.magnitude < 0.2f)
        {
            _spiderM.SpottedTarget = false;

        }
    }

    public void Pathfinding()
    {

        StartNode = _nodeGrid.FindNearestValidNode(_model.transform);
        _endNode = _nodeGrid.FindNearestValidNode(_target.transform);


        if (StartNode != null && _endNode != null)
        {
            var path = _astar.Run(StartNode, Satisfies, GetConnections, GetCost, Heuristic);
            if (path != null && path.Count > 0)
            {
                _spiderM.GoalNode = _endNode;
                _spiderM._startNode = StartNode;
                Wp.AddWaypoints(path);
            }
        }
    }

    public override void Sleep()
    {

        base.Sleep();
        _model.OnRun -= _view.RunAnim;

    }
}
