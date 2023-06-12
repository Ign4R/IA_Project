using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neightbourds;//<- Esto es lo unico que importa

    public void GetNeightbourd(Vector3 dir)
    {     
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 20f))
        {
           
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
            {
                neightbourds.Add(node);
                print("Se obtuvo los vecinos");
            }
        }
              
    }

   

}


