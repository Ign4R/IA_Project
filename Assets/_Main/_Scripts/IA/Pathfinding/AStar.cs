using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<T>
{
    public List<T> Run(T start,
        Func<T, bool> satiesfies,
        Func<T, List<T>> conections,
        Func<T, T, float> getCost,
        Func<T, float> heuristic,
        int watchdog = 100)
    {
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, T> parent = new Dictionary<T, T>();
        Dictionary<T, float> cost = new Dictionary<T, float>();

        pending.Enqueue(start, 0);
        cost[start] = 0;

        while (watchdog > 0 && !pending.IsEmpty)
        {
            watchdog--;
            var curr = pending.Dequeue();
            Debug.Log("ASTAR");
            if (satiesfies(curr))
            {
                var path = new List<T>();
                path.Add(curr);
                while (parent.ContainsKey(path[path.Count - 1]))
                {
                    var father = parent[path[path.Count - 1]];
                    path.Add(father);
                }
                path.Reverse();
                return path;
            }
            visited.Add(curr);
            var neighbours = conections(curr);
            for (int i = 0; i < neighbours.Count; i++)
            {
                var neigh = neighbours[i];
                if (visited.Contains(neigh)) continue;
                float tentativeCost = cost[curr] + getCost(curr, neigh);
                if (cost.ContainsKey(neigh) && cost[neigh] < tentativeCost) continue;
                pending.Enqueue(neigh, tentativeCost + heuristic(neigh));
                parent[neigh] = curr;
                cost[neigh] = tentativeCost;
            }
        }
        return new List<T>();
    }
    public List<T> CleanPath(List<T> path, Func<T, T, bool> inView)
    {
        if (path == null) return path;
        if (path.Count <= 2) return path;
        var list = new List<T>();
        list.Add(path[0]);
        for (int i = 2; i < path.Count - 1; i++)
        {
            var gp = list[list.Count - 1];
            if (!inView(gp, path[i]))
            {
                list.Add(path[i - 1]);
            }
        }
        list.Add(path[path.Count - 1]);
        return list;
    }
}

