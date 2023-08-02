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
    public float _radius;
    public LayerMask _layerMask;
    public bool ignoreNode;
    public void GetNeightbourd(Vector3 dir, int maxDistance)
    {
        lenght = maxDistance;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, maxDistance))
        {
            print(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == _layerMask) return; ///TODO
            var node = hit.collider.GetComponent<Node>();
            if (node == null) return;
            _neightbourds.Add(node);
            print("Se obtuvieron los vecinos");

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
    private void OnDrawGizmos()
    {
        var count = _neightbourds.Count;
        if (count > 0)
            Gizmos.DrawRay(transform.position, _neightbourds[0].transform.position - transform.position);
        if (count > 1)
            Gizmos.DrawRay(transform.position, _neightbourds[1].transform.position - transform.position);
        if (count > 2)
            Gizmos.DrawRay(transform.position, _neightbourds[2].transform.position - transform.position);
        if (count > 3)
            Gizmos.DrawRay(transform.position, _neightbourds[3].transform.position - transform.position);
        //Gizmos.DrawRay(transform.position, Vector3.forward * lenght);
        //Gizmos.DrawRay(transform.position, Vector3.back * lenght);

    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LeaderDetect"))
            ignoreNode = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LeaderDetect"))
            ignoreNode = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }



}


