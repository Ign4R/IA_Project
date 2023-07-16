using System.Collections;
using UnityEngine;
public class TargetFindState<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    LeaderModel _enemyModel;
    Transform _target;
    float _timerValue;

    public TargetFindState(ISteering obsAvoid,NodeGrid nodeGrid, Transform target, float timerState): base(obsAvoid)
    {
        _astar = new AStar<Node>();
        _nodeGrid = nodeGrid;
        _target = target;
        _timerValue = timerState;

    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyModel = (LeaderModel)model;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun;
        _enemyModel._coneOfView.color = Color.blue;
        CurrentTimer = _timerValue;
        Pathfinding();
        _model.LookDir(StartNode.transform.position + Avoid.GetDir().normalized);

        Debug.Log("(start,end near player ) " + StartNode + _endNode);

    }
        
    public override void Execute()
    {     
        Debug.Log("Execute Hunt state");     
        base.Execute();
        Vector3 astarDir = Wp.GetDir() * _enemyModel._multiplierAstar;
        Vector3 avoidDir = Avoid.GetDir() * _enemyModel._multiplierAvoid;
        if (_endNode!=null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);
            Vector3 posEnd = goalNodeFix - _model.transform.position;
            if (CurrentTimer > 0)
            {
                _enemyModel.CurrentTimerHunt = CurrentTimer;
                DecreaseTimer();
                if (posEnd.magnitude < 0.2f)
                {
                    Pathfinding();
                    var newDir = Wp.GetDir() * _enemyModel._multiplierAstar;
                    astarDir = newDir;
                }
            }          
            else if (posEnd.magnitude < 0.2f) 
            {
                _enemyModel.SpottedTarget = false; 

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
            var path =_astar.Run(StartNode, Satisfies, GetConnections, GetCost, Heuristic);
            if (path != null && path.Count > 0)
            {
                _enemyModel.GoalNode = _endNode;
                _enemyModel._startNode = StartNode;
                Wp.AddWaypoints(path);
            }
        }
        else
        {
            Debug.Log("No se encontro los nodos: (start,end) " + StartNode + endNode);
        }

       

    }

    public override void Sleep()
    {
        Debug.Log("Sleep Hunt state");
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
        StartNode = _endNode;
        _enemyModel._startNode = StartNode;
        _enemyModel.SpottedTarget = false;
    }
}
