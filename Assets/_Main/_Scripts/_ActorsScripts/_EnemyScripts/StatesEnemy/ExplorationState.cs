using System.Collections.Generic;
using UnityEngine;

public class ExplorationState<T> : NavigationState<T>
{
 
    NPCLeader_M _npcLeaderM;
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    List<Node> _path;


    public ExplorationState(NodeGrid nodeGrid, Node startNode, ISteering obsAvoid):base(obsAvoid)
    {
        _nodeGrid = nodeGrid;
        StartNode = startNode;
        _astar = new AStar<Node>();
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _npcLeaderM = (NPCLeader_M)model;
    }
    public override void Awake()
    {
        base.Awake();
        _model.OnRun += _view.AnimRun; 
        _model.Move(Vector3.zero);
        _npcLeaderM._coneOfView.color = Color.yellow;

        if (StartNode == null || _nodeGrid == null)
        {
            Debug.LogError("FALTAN REFERENCIAS");
            return;
        }
        Pathfinding(StartNode);
    }
    public override void Execute()
    {
        Debug.Log("Execute Patrol state");
        base.Execute();
        Vector3 astarDir = Wp.GetDir().normalized * _npcLeaderM._multiplierAstar;
        Vector3 avoidDir = Avoid.GetDir().normalized * _npcLeaderM._multiplierAvoid;
        if (CurrentTimer > 0 && _endNode!=null)
        {
            Vector3 goalNode = _endNode.transform.position;
            float endDistance = (goalNode - _model.transform.position).magnitude;
            if (endDistance < 0.2f)
            {

                Pathfinding(_endNode);
                var newDir = Wp.GetDir().normalized * _npcLeaderM._multiplierAstar;
                astarDir = newDir;
            }
        }
   

        Vector3 dirFinal = astarDir + avoidDir;
        _model.Move(dirFinal);
        _model.LookDir(dirFinal);
    }
    public override void Sleep()
    {
        base.Sleep();
        Debug.Log("Sleep Patrol state");
        _model.OnRun -= _view.AnimRun;
    }


    ///TODO: No repetir codigo de pathfinding reutilizar metodo (hacer mas generico)
    public void Pathfinding(Node initialNode)
    {

        StartNode?.RestartMat();
        _endNode?.RestartMat();
        StartNode = null;
        _endNode = _npcLeaderM.GoalNode;
        StartNode = initialNode;

        while (_endNode == StartNode)
        {
            _endNode = _nodeGrid.GetRandomNode();
        }


        _path = _astar.Run(initialNode, Satisfies, GetConnections,
           GetCost, Heuristic, 500);

        if (_path != null && _path.Count > 0)
        {
            StartNode.SetColorNode(Color.white);
            _endNode.SetColorNode(Color.green);
            _npcLeaderM.GoalNode = _endNode;
            _npcLeaderM._startNode = StartNode;
            Wp.AddWaypoints(_path);
        }
    }

   



}