using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public Transform _target;
    public LayerMask _layerMask;
    public Vector3Int _size; /// REFERENCE PLANE 
    public Vector3Int _startPosition;// REFERENCE 
    public GameObject _nodePrefab;
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
            Vector3 nodeSize = new Vector3(2, 1, 2);
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

    public Node GetNodeNearTarget(Vector3 target)
    {
        float bestDistance = 0;
        Node bestNode = null;
        for (int i = 0; i < _allNodes.Count; i++)
        {
            Node currNode = _allNodes[i];
            float currDistance = Vector3.Distance(target, currNode.transform.position);
            if (bestNode == null || bestDistance > currDistance)
            {
                bestDistance = currDistance;
                bestNode = currNode;
            }
        }

        if (bestNode != null)
        {
            Debug.Log("best Node");
            return bestNode;
        }
        else
        {
            Debug.Log("null");
            return null;
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
                print("hola");
                nodeGenerate.GetNeigh();
                EditorUtility.SetDirty(nodeGenerate);
            }


            serializedObject.ApplyModifiedProperties();
        }

    }

    //[CustomEditor(typeof(NodeGrid))]
    //public class NodeGenerateTool : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        NodeGrid nodeGenerate = (NodeGrid)target;


    //        if (GUILayout.Button("Get Node Player"))
    //        {
    //            var target = nodeGenerate._target.position;
    //            print("PLAYER NODE: " + nodeGenerate.GetNodeNearTarget(target).name);
    //        }

    //    }

    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_startPosition, _size);
    }
}
