using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra<T>
{
    public List<T> Run(T start, Func<T, bool> satiesfies, Func<T, List<T>> conections, Func<T, T, float> getCost, int watchdog = 100)
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
            Debug.Log("DIJKSTRA");
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
                pending.Enqueue(neigh, tentativeCost);
                parent[neigh] = curr;
                cost[neigh] = tentativeCost;
            }
        }
        return new List<T>();
    }
}

