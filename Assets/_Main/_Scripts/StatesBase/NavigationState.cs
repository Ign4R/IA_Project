using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationState<T> : EntityStateBase<T>
{
    protected Node _endNode;
    public ISteering Avoid { get; }
    public IWaypoint<Node> Wp { get; protected set; }
    public static Node StartNode { get ;  protected set ; }

    public NavigationState(ISteering obsAvoid)
    {
       

        Avoid = obsAvoid;
    }
    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        Wp = model as IWaypoint<Node>;

    }
    public override void Execute()
    {
        base.Execute();
        Debug.LogWarning(Avoid);
    }

    protected float GetCost(Node parent, Node son)
    {
        float multiplierDistance = 1;
        float cost = 0;
        float multiplierIgnoreNode = 60;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
        if (son.ignoreNode)
            cost += multiplierIgnoreNode;
        return cost;
    }
    protected float Heuristic(Node curr)
    {
        float multiplierDistance = 2;
        float cost = 0;
        cost += Vector3.Distance(curr.transform.position, _endNode.transform.position) * multiplierDistance;
        return cost;
    }
    protected List<Node> GetConnections(Node curr)
    {
        return curr._neightbourds;
    }

    protected bool Satisfies(Node curr)
    {
        return curr == _endNode;
    }



}