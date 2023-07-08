using System.Collections;
using UnityEngine;
public class EnemyStateHunt<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    EnemyModel _enemyModel;
    Transform _target;
    T _inputPatrol;
    float _timerValue;

    public EnemyStateHunt(T inputPatrol, NodeGrid nodeGrid, Transform target, float timerState)
    {
        _astar = new AStar<Node>();
        _inputPatrol = inputPatrol;
        _nodeGrid = nodeGrid;
        _target = target;
        _timerValue = timerState;

    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyModel = (EnemyModel)model;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun;
        _enemyModel._coneOfView.color = Color.magenta;
        CurrentTimer = _timerValue;
        Node startNode = _nodeGrid.GetNodeNearTarget(_model.transform.position);
        Pathfinding(startNode);
        _model.LookDir(startNode.transform.position);

        Debug.Log("(start,end near player ) " + _startNode + _endNode);

    }
        
    public override void Execute()
    {     
        Debug.Log("Hunt State");     
        base.Execute();
        Vector3 dirAstar = Wp.GetDir() * _enemyModel._multiplierAstar;
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
                    Pathfinding(_endNode);
                }
            }          
            else if (posEnd.magnitude < 0.2f) 
            {
                _enemyModel.SpottedTarget = false; 

            }
            

        }
   
        _model.Move(dirAstar);
        _model.LookDir(dirAstar);
    }

    public void Pathfinding(Node initialNode)
    {
        Node endNode = _nodeGrid.GetNodeNearTarget(_target.position);
        _endNode = endNode;
        _startNode = initialNode;
        if (_startNode != null && _endNode != null)
        {
            var path =_astar.Run(_startNode, Satisfies, GetConnections, GetCost, Heuristic);
            if (path != null && path.Count > 0)
            {
                _enemyModel.GoalNode = _endNode;
                _enemyModel._startNode = _startNode;
                Wp.AddWaypoints(path);
            }
        }
        else
        {
            Debug.Log("No se encontro los nodos: (start,end) " + initialNode + endNode);
        }

       

    }

    public override void Sleep()
    {
        base.Sleep();
        _model.OnRun -= _view.AnimRun;
        _enemyModel.SpottedTarget = false;
    }
}
