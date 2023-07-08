using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{
    EnemyModel _enemyModel;
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    List<Node> _path;


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
        _model.Move(Vector3.zero);
        _model.OnRun += _view.AnimRun; 
        _enemyModel._coneOfView.color = Color.yellow;

        if (_startNode != null)
        {
            _model.LookDir(_startNode.transform.position);
            Pathfinding(_startNode);
        }
    }
    public override void Execute()
    {
        Debug.Log("Execute Patrol state");
        base.Execute();
        Vector3 dirAstar = Wp.GetDir() * _enemyModel._multiplierAstar;
        if (_endNode != null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);

            Vector3 posEnd = goalNodeFix - _model.transform.position;

            if (posEnd.magnitude < 0.2f)
            {
                Pathfinding(_endNode);
                var newDir = Wp.GetDir() * _enemyModel._multiplierAstar;
                dirAstar = newDir;
            }
        }
        Vector3 dirBalanced = dirAstar.normalized;
        _model.Move(dirBalanced);
        _model.LookDir(dirBalanced);

    }
    public override void Sleep()
    {
        base.Sleep();
        Debug.Log("Sleep Patrol state");
        _model.OnRun -= _view.AnimRun;
    }
    public void Pathfinding(Node initialNode)
    {

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

   



}