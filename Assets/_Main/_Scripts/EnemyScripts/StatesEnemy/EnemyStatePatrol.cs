using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{
    BFS<Node> _astar;
    NodeGrid _nodeGrid;
    List<Node> _path;
    Node _startNode;
    Node _endNode;


    public EnemyStatePatrol(NodeGrid nodeGrid)
    {
        _nodeGrid = nodeGrid;

    }
    public override void Awake()
    {       
        base.Awake();
        _model.OnRun += _view.AnimRun;
        _astar = new BFS<Node>(); ///TODO: CAMBIAR POR ASTARPLUS IMPORTANTE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _startNode = _nodeGrid.GetRandomNode();
        var startNode = _startNode.transform.position;  ///asignar el start node a mano
        startNode.y = _model.transform.localPosition.y;
        _model.transform.position = startNode;
        Pathfinding(_startNode);


    }

    public override void Execute()
    {
        Debug.Log("Patrol State");
        base.Execute();
        var dir = Wp.GetDir();

        Vector3 goalNode = _endNode.transform.position;
        Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);

        var dirEnd = goalNodeFix - _model.transform.position;


        if (dirEnd.magnitude < 0.2f) 
        {
            Pathfinding(_endNode);
        }
        _model.Move(dir);
        _model.LookDir(dir);

    }

    public void Pathfinding(Node initialNode)
    {
        Debug.Log("Enter new pathfinding");

        _startNode.RestartMat();
        _startNode = initialNode;

        if (_nodeGrid != null)
        {
            _endNode = _nodeGrid.GetRandomNode();

            _endNode._mat.color = Color.green;
            _startNode._mat.color = Color.white;

            if (_endNode == initialNode)
            {
                _endNode = _nodeGrid.GetRandomNode();
                _enemyModel._goalNode = _endNode;
            }

            _path = _astar.Run(initialNode, Satisfies, GetConnections, 500);
            if (_path != null && _path.Count > 0)
            {
                _endNode = _path[_path.Count - 1]; // Almacenar el último nodo del camino
                _enemyModel._goalNode = _endNode;
                Wp.AddWaypoints(_path);
            }
        }
    }

    List<Node> GetConnections(Node curr)
    {
        return curr._neightbourds;
    }
    public bool Satisfies(Node curr)
    {
        return curr == _endNode;
    }

    public override void Sleep()
    {
        base.Sleep();
        _model.Move(Vector3.zero);
    }
}
