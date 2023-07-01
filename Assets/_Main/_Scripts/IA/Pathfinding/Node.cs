using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> _neightbourds;//<- Esto es lo unico que importa
    public int lenght;
    public  Material _mat;
    public Color _color;


    public void GetNeightbourd(Vector3 dir, int maxDistance)
    {
        lenght = maxDistance;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, maxDistance))
        {
           
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
            {
                _neightbourds.Add(node);
                print("Se obtuvo los vecinos");
            }
        }
              
    }

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _color=_mat.color;
    }
    public void RestartMat()
    {
        _mat.color = _color;
    }
    public void SetColorNode(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, Vector3.right * lenght);
        Gizmos.DrawRay(transform.position, Vector3.left * lenght);
        Gizmos.DrawRay(transform.position, Vector3.forward * lenght);
        Gizmos.DrawRay(transform.position, Vector3.back * lenght);
    
    }



}


