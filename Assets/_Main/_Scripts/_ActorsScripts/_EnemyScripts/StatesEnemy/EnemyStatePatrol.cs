using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{
    EnemyModel _enemyModel;
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    List<Node> _path;
    Node _endNode;
    Node _startNode;

    public EnemyStatePatrol(NodeGrid nodeGrid, Node startNode)
    {
        _nodeGrid = nodeGrid;
        _startNode = startNode;
        _astar = new AStar<Node>();
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _enemyModel = (EnemyModel)model;
    }
    public override void Awake()
    {       
        base.Awake();
        _enemyModel._coneOfView.SetActive(true);
        _model.OnRun += _view.AnimRun;    
        if (_startNode != null) 
        {
            _model.LookDir(_startNode.transform.position);
            Pathfinding(_startNode);
            _startNode = null;
        }
        
    }
    public override void Execute()
    {
        Debug.Log("Patrol State");
        base.Execute();
        float multiplierAvoid= _enemyModel._multiplierAvoid;
        Vector3 dirAvoid = _enemyModel.ObsAvoidance.GetDir() * multiplierAvoid;
        Vector3 dirAstar = Wp.GetDir() * _enemyModel._multiplierAstar;
        if (_endNode != null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);

            Vector3 posEnd = goalNodeFix - _model.transform.position;

            if (posEnd.magnitude < 0.2f)
            {
                Pathfinding(_endNode);           
            }
        }
        Vector3 dirBalanced = (dirAstar.normalized + dirAvoid.normalized);
        _model.Move(dirBalanced);
        _model.LookDir(dirBalanced);

    }
    public override void Sleep()
    {
        base.Sleep();
        _model.Move(Vector3.zero);
        _model.OnRun -= _view.AnimRun;
    }
    public void Pathfinding(Node initialNode)
    {
        Debug.Log("Enter new pathfinding");
        _startNode?.RestartMat();
        _startNode = initialNode;
        _endNode?.RestartMat();
        _endNode = _nodeGrid.GetRandomNode();

        while (_endNode == initialNode)
        {
            _endNode = _nodeGrid.GetRandomNode();
        }
        _path = _astar.Run(initialNode, Satisfies, GetConnections,
           GetCost, Heuristic, 500);
        if (_path != null && _path.Count > 0)
        {
            _startNode.SetColorNode(Color.white);
            _endNode.SetColorNode(Color.green);
            _enemyModel.GoalNode = _endNode;
            _enemyModel._startNode = _startNode;
            Wp.AddWaypoints(_path);
        }
    }

    public float GetCost(Node parent, Node son)
    {
        float multiplierDistance = 1;
        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
        return cost;
    }
    public float Heuristic(Node curr)
    {
        float multiplierDistance = 2;
        float cost = 0;
        cost += Vector3.Distance(curr.transform.position, _endNode.transform.position) * multiplierDistance;
        return cost;
    }
    public List<Node> GetConnections(Node curr)
    {
        return curr._neightbourds;
    }

    public bool Satisfies(Node curr)
    {
        return curr == _endNode;
    }

    public bool InView(Node from, Node to)
    {
        if (Physics.Linecast(from.transform.position, to.transform.position, 8)) return false;
        return true;
    }



}