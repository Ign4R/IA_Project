using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThetaStar<T>
{
    public List<T> Run(T start,
     Func<T, bool> satiesfies,
     Func<T, List<T>> conections,
     Func<T, T, float> getCost,
     Func<T, float> heuristic,
     Func<T, T, bool> inView,
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
            Debug.Log("THETA");
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
                T realParent = curr;
                if (parent.ContainsKey(curr) && inView(parent[curr], neigh))
                {
                    realParent = parent[curr];
                }
                float tentativeCost = cost[realParent] + getCost(realParent, neigh);
                if (cost.ContainsKey(neigh) && cost[neigh] < tentativeCost) continue;
                pending.Enqueue(neigh, tentativeCost + heuristic(neigh));
                parent[neigh] = realParent;
                cost[neigh] = tentativeCost;
            }
        }
        return new List<T>();
    }
}

