using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS<T>
{
    public List<T> Run(T start, Func<T, bool> satiesfies, Func<T, List<T>> conections, int watchdog = 100)
    {
        Stack<T> pending = new Stack<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, T> parent = new Dictionary<T, T>();

        pending.Push(start);

        while (watchdog > 0 && pending.Count > 0)
        {
            watchdog--;
            var curr = pending.Pop();
            Debug.Log("DFS");
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
                pending.Push(neigh);
                parent[neigh] = curr;
            }
        }
        return new List<T>();
    }
}
