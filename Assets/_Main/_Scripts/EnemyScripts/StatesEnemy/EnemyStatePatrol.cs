using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{
    AStar<Node> _astar;
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
        _astar = new AStar<Node>(); ///TODO: CAMBIAR POR ASTARPLUS IMPORTANTE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        if (_nodeGrid!=null)
        {
            _startNode = _nodeGrid.GetRandomNode();
            var startNode = _startNode.transform.position;  ///asignar el start node a mano
            startNode.y = _model.transform.localPosition.y;
            _model.transform.position = startNode;
        }

        Pathfinding(_startNode);

      


    }

    public override void Execute()
    {
        Debug.Log("Patrol State");
        base.Execute();
        var dir = Wp.GetDir();
        if (_endNode != null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);

            Vector3 posEnd = goalNodeFix - _model.transform.position;

            if (posEnd.magnitude < 0.2f)
            {
                Pathfinding(_endNode);
                dir = Wp.GetDir(); ///recalculamos la direccion para evitar errores
            }
        }
        _model.Move(dir);
        _model.LookDir(dir);

    }

    public void Pathfinding(Node initialNode)
    {
        Debug.Log("Enter new pathfinding");

        _startNode?.RestartMat();
        _startNode = initialNode;

        if (_endNode != null)
        {
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
                _enemyModel._goalNode = _endNode;
                _enemyModel._startNode = _startNode;
                Wp.AddWaypoints(_path);
            }
        }
    }

    public float GetCost(Node parent, Node son)
    {
        float multiplierDistance = 1;
        //float multiplierEnemies = 20;
        float multiplierTrap = 20;

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
        if (son.hasTrap)
            cost += multiplierTrap;
        //cost += 100 * multiplierEnemies;
        return cost;
    }
    public float Heuristic(Node curr)
    {
        float multiplierDistance = 2;
        float cost = 0;
        ///cur.cost
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
        Debug.Log("CLEAN");
        if (Physics.Linecast(from.transform.position, to.transform.position, 8)) return false;
        //Distance
        //Angle
        return true;
    }


    public override void Sleep()
    {
        base.Sleep();
        _model.Move(Vector3.zero);
    }
}
