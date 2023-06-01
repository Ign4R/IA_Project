using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS<T>
{
    //delegate bool satiesfies(T node);
    public List<T> Run(T start, Func<T, bool> satiesfies, Func<T, List<T>> conections, int watchdog = 100)
    {
        //pending.Enqueue(start);
        //var curr = pending.Peek();
        //var curr2 = pending.Dequeue();
        //visited.add(curr)
        //visited.remove(curr)
        //visited.contains(curr)
        Queue<T> pending = new Queue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, T> parent = new Dictionary<T, T>();

        pending.Enqueue(start);

        while (watchdog > 0 && pending.Count > 0)
        {
            watchdog--;
            var curr = pending.Dequeue();
            Debug.Log("BFS");
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
                pending.Enqueue(neigh);
                parent[neigh] = curr;
            }
        }
        return new List<T>();
    }
}
