using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public GameObject _plane;

    public GameObject _node;

    List<Node> _allNodes = new List<Node>();


    public void Generate()
    {
        _allNodes = new List<Node>();
        GameObject parentNode = new GameObject("NodeParent");
        parentNode.transform.parent = transform;
        Vector3 planeSize = _plane.transform.localScale;
        Vector3 startPosition = _plane.transform.position;
        Vector3 nodeSize = new Vector3(5, 1, 5);
        int nodeSpacing = 10;
        int nodesNumberX = (int)planeSize.x;
        int nodesNumberZ = (int)planeSize.z;

        for (int x = 0; x < nodesNumberX; x++)
        {
            for (int z = 0; z < nodesNumberZ; z++)
            {
                var centerX = (x - (nodesNumberX - 1) / 2f);
                var centerZ = (z - (nodesNumberZ - 1) / 2f);

                Vector3 position = startPosition + new Vector3(centerX * nodeSpacing, 1f, centerZ * nodeSpacing);

                GameObject prefab = Instantiate(_node);
                prefab.name = "Node" + (x * nodesNumberZ + z + 1);
                prefab.transform.localScale = nodeSize;
                prefab.transform.position = position;            
                prefab.transform.parent = parentNode.transform;
                var node = prefab.GetComponent<Node>();
                _allNodes.Add(node);
            }
        }

    }

    public void GetNeigh()
    {
        foreach (var node in _allNodes)
        {
            if (node != null)
            {
                node.GetNeightbourd(Vector3.forward);
                node.GetNeightbourd(Vector3.back);
                node.GetNeightbourd(Vector3.left);
                node.GetNeightbourd(Vector3.right);
            }
            else
            {
                _allNodes = new List<Node>();
            }
        }

    }
    [CustomEditor(typeof(NodeGrid))]
    public class NodeGenerateTool : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            NodeGrid nodeGenerate = (NodeGrid)target;

          
            if (GUILayout.Button("Generate Nodes"))
            {
                nodeGenerate.Generate();

            }

        
            if (GUILayout.Button("Get Neigh"))
            {
                nodeGenerate.GetNeigh();
            }
        }
    }
}
