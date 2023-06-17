using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class AgentController : MonoBehaviour
//{
//    public CI_Model crash;
//    public Box box;
//    public Node goalNode;
//    public Node startNode;
//    public float radius;
//    Collider[] _colliders;
//    public LayerMask maskNodes;
//    public LayerMask maskObs;
//    List<Vector3> lastPathTest;

//    [Header("Vector")]
//    public float range;

//    BFS<Node> _bfs;
//    DFS<Node> _dfs;
//    Dijkstra<Node> _djk;
//    AStar<Node> _ast;
//    AStar<Vector3> _astVector;
//    ThetaStar<Node> _theta;

//    private void Awake()
//    {
//        _bfs = new BFS<Node>();
//        _dfs = new DFS<Node>();
//        _djk = new Dijkstra<Node>();
//        _ast = new AStar<Node>();
//        _astVector = new AStar<Vector3>();
//        _theta = new ThetaStar<Node>();
//        _colliders = new Collider[10];
//    }
//    public void BFSRun()
//    {
//        var start = startNode;
//        if (start == null) return;

//        var path = _bfs.Run(start, Satisfies, GetConections, 500);

//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    public void DFSRun()
//    {
//        var start = startNode;
//        if (start == null) return;

//        var path = _dfs.Run(start, Satisfies, GetConections, 500);

//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    public void DijkstraRun()
//    {
//        var start = startNode;
//        if (start == null) return;

//        var path = _djk.Run(start, Satisfies, GetConections, GetCost, 500);

//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    public void AStarRun()
//    {
//        var start = startNode;
//        if (start == null) return;

//        var path = _ast.Run(start, Satisfies, GetConections, GetCost, Heuristic, 500);

//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    public void AStarPlusRun()
//    {
//        var start = startNode;
//        if (start == null) return;
//        var path = _ast.Run(start, Satisfies, GetConections, GetCost, Heuristic, 500);
//        path = _ast.CleanPath(path, InView);
//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    public void AStarPlusVectorRun()
//    {
//        var posStart = crash.transform.position;
//        var path = _astVector.Run(posStart, Satisfies, GetConections, GetCost, Heuristic, 500);
//        path = _astVector.CleanPath(path, InView);
//        lastPathTest = path;
//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    bool Satisfies(Vector3 curr)
//    {
//        var distance = Vector3.Distance(curr, box.transform.position);
//        if (distance > range) return false;
//        if (Physics.Linecast(curr, box.transform.position, maskObs)) return false;
//        return true;
//    }
//    List<Vector3> GetConections(Vector3 curr)
//    {
//        EJEMPLO SI ESTO ESTA EN EL PARCIAL... Seras el equivalente a M
//        var list = new List<Vector3>();
//        for (int x = -1; x <= 1; x++)
//        {
//            for (int y = -1; y <= 1; y++)
//            {
//                if (x == y) continue;
//                if (x == y || x == -y) continue;
//                var newPos = curr + new Vector3(x, 0, y);
//                if (InView(curr, newPos)) //ESTO ESTA MUY MUCHO MAS MALLL 
//                {
//                    list.Add(newPos);
//                }
//            }
//        }

//        return list;
//    }
//    bool InView(Vector3 from, Vector3 to)
//    {
//        Debug.Log("CLEAN");
//        if (Physics.Linecast(from, to, maskObs)) return false;
//        Distance
//        Angle
//        return true;
//    }

//    float GetCost(Vector3 parent, Vector3 son)
//    {
//        float multiplierDistance = 1;
//        float cost = 0;
//        cost += Vector3.Distance(parent, son) * multiplierDistance;
//        return cost;
//    }

//    float Heuristic(Vector3 curr)
//    {
//        float multiplierDistance = 2;
//        float cost = 0;
//        cost += Vector3.Distance(curr, box.transform.position) * multiplierDistance;
//        return cost;
//    }

//    public void ThetaStarRun()
//    {
//        var start = startNode;
//        if (start == null) return;

//        var path = _theta.Run(start, Satisfies, GetConections, GetCost, Heuristic, InView, 2000);

//        crash.SetWayPoints(path);
//        box.SetWayPoints(path);
//    }
//    bool InView(Node from, Node to)
//    {
//        Debug.Log("CLEAN");
//        if (Physics.Linecast(from.transform.position, to.transform.position, maskObs)) return false;
//        Distance
//        Angle
//        return true;
//    }
//    float Heuristic(Node curr)
//    {
//        float multiplierDistance = 2;
//        float cost = 0;
//        cost += Vector3.Distance(curr.transform.position, goalNode.transform.position) * multiplierDistance;
//        return cost;
//    }
//    float GetCost(Node parent, Node son)
//    {
//        float multiplierDistance = 1;
//        float multiplierEnemies = 20;
//        float multiplierTrap = 20;

//        float cost = 0;
//        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
//        if (son.hasTrap)
//            cost += multiplierTrap;
//        cost += 100 * multiplierEnemies;
//        return cost;
//    }
//    List<Node> GetConections(Node curr)
//    {
//        return curr.neightbourds;
//    }
//    bool Satisfies(Node curr)
//    {
//        return curr == goalNode;
//    }
//    Node GetStartNode()
//    {
//        int count = Physics.OverlapSphereNonAlloc(crash.transform.position, radius, _colliders, maskNodes);
//        float bestDistance = 0;
//        Collider bestCollider = null;
//        for (int i = 0; i < count; i++)
//        {
//            Collider currColl = _colliders[i];
//            float currDistance = Vector3.Distance(crash.transform.position, currColl.transform.position);
//            if (bestCollider == null || bestDistance > currDistance)
//            {
//                bestDistance = currDistance;
//                bestCollider = currColl;
//            }
//        }
//        if (bestCollider != null)
//        {
//            return bestCollider.GetComponent<Node>();
//        }
//        else
//        {
//            return null;
//        }
//    }
//    private void OnDrawGizmos()
//    {
//        if (lastPathTest != null)
//        {
//            Gizmos.color = Color.green;
//            for (int i = 0; i < lastPathTest.Count - 2; i++)
//            {
//                Gizmos.DrawLine(lastPathTest[i], lastPathTest[i + 1]);
//            }
//        }
//    }
//}
