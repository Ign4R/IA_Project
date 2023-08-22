using System.Collections;
using UnityEngine;
public class FindHomeState<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    NPCLeader_M _npcLeaderM;
    Node _target;

    public FindHomeState(ISteering obsAvoid,NodeGrid nodeGrid, Node target): base(obsAvoid)
    {
        _astar = new AStar<Node>();
        _nodeGrid = nodeGrid;
        _target = target;

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
        _npcLeaderM._coneOfView.color = Color.clear;
        _model.Move(Vector3.zero);
        Pathfinding();
        var temp= _nodeGrid.FindNearestValidNode(_npcLeaderM);
        _model.LookDir(temp.transform.position);


    }

    public override void Execute()
    {
        base.Execute();
        Vector3 astarDir = Wp.GetDir() * _npcLeaderM._multiplierAstar;
        Vector3 avoidDir = Avoid.GetDir() * _npcLeaderM._multiplierAvoid;


        Vector3 dirFinal = astarDir.normalized + avoidDir.normalized;
        _model.Move(dirFinal);
        _model.LookDir(dirFinal*2);

    }

    public void Pathfinding()
    {

        StartNode = _nodeGrid.FindNearestValidNode(_npcLeaderM);
        _endNode = _target;


        if (StartNode != null && _endNode != null)
        {
            var path =_astar.Run(StartNode, Satisfies, GetConnections, GetCost, Heuristic);
            if (path != null && path.Count > 0)
            {
                _npcLeaderM.GoalNode = _endNode;
                _npcLeaderM._startNode = StartNode;
                Wp.AddWaypoints(path);
            }
        }
        else
        {
            Debug.Log("No se encontro los nodos: (start,end) " + StartNode + _endNode);
        }

       

    }

    public override void Sleep()
    {
        Debug.Log("Sleep FindZone state");
        base.Sleep();
        _model.OnRun -= _view.AnimRun;

    }
}
