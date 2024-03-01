using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public LayerMask nodeLayer;
    [ReadOnly]public float radius = 4;
    public Transform _target;
    public Vector3Int _size; /// REFERENCE PLANE 
    public Vector3Int _startPosition;// REFERENCE 
    public GameObject _nodePrefab;
    private int _nodeSpacing;
    [Range(3, 10)]
    public int _nodeCount=3;
    public List<Node> _allNodes;
    private Collider[] _colliders;

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
            Vector3 nodeSize = new Vector3(2, 1, 2); ///TODO
            Vector3Int extents = _size / 2;
            Vector3Int center = _startPosition;
            Vector3Int min = center - extents;
            Vector3Int max = center + extents;
            Vector3Int nodeLength = _size / (_nodeCount-1);
            int nodeIndex = 0;

            _nodeSpacing = nodeLength.x;
            for (int x = min.x; x <= max.x; x += nodeLength.x)
            {
                for (int z = min.z; z <= max.z; z += nodeLength.z)
                {
                    Vector3Int position = new Vector3Int(x, 0, z);
                    Collider[] temp;
                    temp = new Collider[10];
                    int colliderCount = Physics.OverlapSphereNonAlloc(position, radius, temp, ignoreLayer);
                    if (colliderCount == 0)
                    {
                        GameObject prefab = Instantiate(_nodePrefab);
                        prefab.name = "Node" + nodeIndex++;
                        prefab.transform.localScale = nodeSize;
                        prefab.transform.position = position;
                        prefab.transform.parent = parentNode.transform;
                        var node = prefab.GetComponent<Node>();
                        node._radius = radius;
                        _allNodes.Add(node);
                    }
                    else
                    {
                        print("no se genero el nodo: " + nodeIndex++);

                    }

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
                node.GetNeightbourd(Vector3.forward, _size.z);
                node.GetNeightbourd(Vector3.back, _size.z);
                node.GetNeightbourd(Vector3.left, _size.z);
                node.GetNeightbourd(Vector3.right, _size.z);
            }
         
        }
    }
    public Node FindNearestValidNode(Transform npc)
    {
        float bestDistance = 0; // Inicializamos en 0 asumiendo que el primer nodo es el más cercano
        Node nearestNode = null;

        Vector3 npcPosition = npc.transform.position;
        Vector3 npcBackward = -npc.transform.forward; // Dirección hacia atrás del NPC

        for (int i = 0; i < _allNodes.Count; i++)
        {
            Node node = _allNodes[i];
            if (node != null)
            {
                // Calculamos la distancia del NPC al nodo en cuestión
                float currDistance = Vector3.Distance(npcPosition, node.transform.position);

                // Calculamos el ángulo entre la dirección hacia atrás del NPC y el vector que apunta desde el NPC al nodo
                //Vector3 directionToNode = node.transform.position - npcPosition;
                //float angleToNode = Vector3.Angle(npcBackward, directionToNode);

                // Si el ángulo es menor que 90 grados (es decir, el nodo está detrás del NPC)
                // y es el nodo más cercano encontrado hasta ahora, actualizamos los valores
                if (bestDistance == 0 || currDistance < bestDistance)
                {
                    bestDistance = currDistance;
                    nearestNode = node;
                }
            }
        }

        return nearestNode;
    }
    public Node GetNodeNearTarget(Transform target, Node ignoredNode = null)
    {
        float bestDistance = 0;
        Node bestNode = null;

        for (int i = 0; i < _allNodes.Count; i++)
        {
            Node currNode = _allNodes[i];
            if (currNode == ignoredNode) continue;
            Vector3 nodePosition = currNode.transform.position;
            nodePosition.y = 0;

            // Calcula la distancia entre el objetivo y el nodo
            float currDistance = Vector3.Distance(target.position, nodePosition);

            // Calcula el vector de dirección desde el nodo hacia el objetivo
            Vector3 directionToTarget = (target.position - nodePosition).normalized;

            // Calcula el ángulo entre el forward del objetivo y la dirección hacia el objetivo
            float angleToTarget = Vector3.Angle(target.forward, directionToTarget);

            // Comprueba si el nodo es el más cercano y está delante del objetivo (según el forward del objetivo)
            if (bestNode == null || currDistance < bestDistance || (currDistance == bestDistance && angleToTarget < 180))
            {
                bestDistance = currDistance;
                bestNode = currNode;
            }
        }

        if (bestNode != null)
        {
            return bestNode;
        }
        else
        {

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_startPosition, _size);
    }
}
