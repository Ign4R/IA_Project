using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationState<T> : EntityStateBase<T>
{
    protected Node _endNode;
    protected Node _startNode;
    public IWaypoint<Node> Wp { get; protected set; }

    public float CurrentTimer { get; protected set; }
    public float TimerValue { get; private set; }

    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        Wp = model as IWaypoint<Node>;

    }
    public virtual void SetTimer(float timer)
    {
        
    }
    protected int SetRandomTimer(float maxFloat)
    {
        int timer = Random.Range(1, (int)maxFloat);
        return timer;
    }

    public void DecreaseTimer()
    {
        CurrentTimer -= Time.deltaTime;

    }

    protected float GetCost(Node parent, Node son)
    {
        float multiplierDistance = 1;
        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
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

    protected bool InView(Node from, Node to)
    {
        if (Physics.Linecast(from.transform.position, to.transform.position, 8)) return false;
        return true;
    }

}