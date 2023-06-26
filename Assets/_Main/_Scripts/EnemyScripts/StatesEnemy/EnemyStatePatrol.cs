using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    List<Node> _path;
    Node _endNode;
    Node _startNode;
    ObstacleAvoidance _avoidance;
    private Vector3 _smoothedDir;

    public EnemyStatePatrol(NodeGrid nodeGrid, Node startNode, ObstacleAvoidance avoidance)
    {
        _nodeGrid = nodeGrid;
        _startNode = startNode;
        _avoidance = avoidance;

    }
    public override void Awake()
    {       
        base.Awake();
        _model.OnRun += _view.AnimRun;
        _astar = new AStar<Node>(); ///TODO: CAMBIAR POR ASTARPLUS IMPORTANTE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        if (_startNode != null) 
        {
            //_startNode = _nodeGrid._startNode;
            //var startNode = _startNode.transform.position;  ///asignar el start node a mano
            //startNode.y = _model.transform.localPosition.y;
            //_model.transform.position = startNode;
            Pathfinding(_startNode);
            _startNode = null;
        }
        
    }

    public override void Execute()
    {
        Debug.Log("Patrol State");

        Debug.LogWarning("AVOIDANCE: " + _avoidance.GetDir());
        base.Execute();

        Vector3 dirAvoid = _avoidance.GetDir() * _enemyModel._multiplierAvoid;
        Vector3 dirAstar = Wp.GetDir() * _enemyModel._multiplierAstar;
        
    

        if (_endNode != null)
        {
            Vector3 goalNode = _endNode.transform.position;
            Vector3 goalNodeFix = new Vector3(goalNode.x, _model.transform.position.y, goalNode.z);

            Vector3 posEnd = goalNodeFix - _model.transform.position;

            if (posEnd.magnitude < 0.2f)
            {
                Pathfinding(_endNode);
                //dir = Wp.GetDir();*/ ///recalculamos la direccion para evitar errores
              
            }
        }

        Vector3 dirBalanced = (dirAstar + dirAvoid).normalized;

        _smoothedDir = Vector3.Lerp(_smoothedDir, dirBalanced, 0.3f*Time.deltaTime).normalized;

        _model.Move(dirBalanced);
        _model.LookDir(dirBalanced);

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
            _enemyModel._goalNode = _endNode;
            _enemyModel._startNode = _startNode;
            Wp.AddWaypoints(_path);
        }

    }

    public float GetCost(Node parent, Node son)
    {
        float multiplierDistance = 1;
        //float multiplierEnemies = 20;
   

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;

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
