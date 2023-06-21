using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask _layerMask;
    public Vector3Int _size; /// REFERENCE PLANE 
    public Vector3Int _startPosition;// REFERENCE 
    public GameObject _nodePrefab;

    public Node _startNode;

    private int _nodeSpacing;
    [Range(3, 10)]
    public int _nodeCount=3;
    public List<Node> _allNodes;



    public List<Node> AllNodes { get => _allNodes; }



    public void Generate()
    {
        _size.y = 0;
        if (_size.sqrMagnitude>=2)
        {
            _allNodes = new List<Node>();
            string nameParent = "NodeParent";
            GameObject parentNode = GameObject.Find(nameParent);
            if (parentNode != null)
            {
                DestroyImmediate(parentNode);///OJO
            }
            parentNode = new GameObject(nameParent);
            parentNode.transform.parent = transform;
            Vector3 nodeSize = new Vector3(5, 1, 5);
            Vector3Int extents = _size / 2;
            Vector3Int center = _startPosition;
            Vector3Int min = center - extents;
            Vector3Int max = center + extents;
            Vector3Int nodeLenght = _size / (_nodeCount-1);
            int nodeIndex = 0;

            _nodeSpacing = nodeLenght.x;
            for (int x = min.x; x <= max.x; x += nodeLenght.x)
            {
                for (int z = min.z; z <= max.z; z += nodeLenght.z)
                {
                    Vector3Int position = new Vector3Int(x, 1, z);
                    GameObject prefab = Instantiate(_nodePrefab);
                    prefab.name = "Node" + nodeIndex++;
                    prefab.transform.localScale = nodeSize;
                    prefab.transform.position = position;
                    prefab.transform.parent = parentNode.transform;
                    var node = prefab.GetComponent<Node>();
                    _allNodes.Add(node);
                }
            }
            
        }




    }

    // ...

    public Node GetRandomNode()
    {
        var iRandom = Random.Range(0, _allNodes.Count);
        return _allNodes[iRandom];
    }


    public void GetNeigh()
    {
        foreach (var node in _allNodes)
        {
            if (node != null && node._neightbourds.Count < 1) 
            {
                node.GetNeightbourd(Vector3.forward, _nodeSpacing);
                node.GetNeightbourd(Vector3.back, _nodeSpacing);
                node.GetNeightbourd(Vector3.left, _nodeSpacing);
                node.GetNeightbourd(Vector3.right, _nodeSpacing);
            }
         
        }
    }



    [CustomEditor(typeof(NodeGrid))]
    public class NodeGenerateTool : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NodeGrid nodeGenerate = (NodeGrid)target;



            if (GUILayout.Button("Generate Nodes"))
            {
                nodeGenerate.Generate();
                EditorUtility.SetDirty(nodeGenerate);
            }



            if (GUILayout.Button("Get Neigh"))
            {
                nodeGenerate.GetNeigh();
                EditorUtility.SetDirty(nodeGenerate);
            }


            serializedObject.ApplyModifiedProperties();
        }
        
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_startPosition, _size);
    }
}
