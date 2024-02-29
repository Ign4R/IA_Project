using System;
using System.Collections;
using UnityEngine;
public class FindHomeState<T> : NavigationState<T>
{
    AStar<Node> _astar;
    NodeGrid _nodeGrid;
    NPCLeader_M _npcLeaderM;
    Node _goal;
    Transform _target;

    public FindHomeState(ISteering obsAvoid,NodeGrid nodeGrid, Node goal,Transform target): base(obsAvoid)
    {
        _astar = new AStar<Node>();
        _nodeGrid = nodeGrid;
        _goal = goal;
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
        _model.OnRun += _view.RunAnim;
        _npcLeaderM._coneOfView.color = Color.clear;
        _model.Move(Vector3.zero);
        Pathfinding();
        //var startNode= _nodeGrid.FindNearestValidNode(_model.transform);
        //_model.LookDir(startNode.transform.position);


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

        StartNode = _nodeGrid.FindNearestValidNode(_model.transform);
        _endNode = _goal;


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
    
        }

       

    }

    public override void Sleep()
    {

        base.Sleep();
        _model.OnRun -= _view.RunAnim;

    }
}
