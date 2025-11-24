using UnityEngine;

[ExecuteAlways]
public class ConnectNodesWithLines : MonoBehaviour
{
    [Tooltip("Set transforms for nodes 0..20 in order")]
    public Transform[] nodes = new Transform[21];

    [Tooltip("Pairs of node indices to connect (e.g. (0,1) )")]
    public Vector2Int[] edges;

    [Header("Line Appearance")]
    public float width = 0.05f;
    public Material lineMaterial; 
    public bool updateEveryFrame = true;

    
    private GameObject[] lineObjects;

    void OnValidate()
    {
        
        if (nodes == null) nodes = new Transform[21];
        CreateOrUpdateLines();
    }

    void Start()
    {
        if (lineMaterial == null)
        {
            
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
        }
        CreateOrUpdateLines();
    }

    void Update()
    {
        if (updateEveryFrame)
            UpdateLinePositions();
    }

    void CreateOrUpdateLines()
    {
        
        if (lineObjects == null || lineObjects.Length != edges.Length)
        {
            if (lineObjects != null)
            {
                for (int i = 0; i < lineObjects.Length; i++)
                    if (lineObjects[i] != null) DestroyImmediate(lineObjects[i]);
            }

            lineObjects = new GameObject[edges.Length];
        }

        for (int i = 0; i < edges.Length; i++)
        {
            if (lineObjects[i] == null)
            {
                var go = new GameObject("Edge_" + i + "_" + edges[i].x + "_" + edges[i].y);
                go.transform.SetParent(transform, false);
                var lr = go.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.useWorldSpace = true;
                lr.material = lineMaterial;
                lr.startWidth = width;
                lr.endWidth = width;
                lr.numCapVertices = 8;
                lr.numCornerVertices = 8;
                lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lr.receiveShadows = false;
                lr.loop = false;
                lr.alignment = LineAlignment.View; 
                lr.startColor = Color.green;
                lr.endColor = Color.green;
                lineObjects[i] = go;
            }

            
            var lrComp = lineObjects[i].GetComponent<LineRenderer>();
            int a = edges[i].x;
            int b = edges[i].y;
            Vector3 pa = (IsValidIndex(a) && nodes[a] != null) ? nodes[a].position : Vector3.zero;
            Vector3 pb = (IsValidIndex(b) && nodes[b] != null) ? nodes[b].position : Vector3.zero;
            lrComp.SetPosition(0, pa);
            lrComp.SetPosition(1, pb);
            lrComp.startWidth = width;
            lrComp.endWidth = width;
            lrComp.material = lineMaterial;
        }
    }

    void UpdateLinePositions()
    {
        if (lineObjects == null) return;
        for (int i = 0; i < edges.Length; i++)
        {
            if (lineObjects[i] == null) continue;
            var lr = lineObjects[i].GetComponent<LineRenderer>();
            int a = edges[i].x;
            int b = edges[i].y;
            Vector3 pa = (IsValidIndex(a) && nodes[a] != null) ? nodes[a].position : Vector3.zero;
            Vector3 pb = (IsValidIndex(b) && nodes[b] != null) ? nodes[b].position : Vector3.zero;
            lr.SetPosition(0, pa);
            lr.SetPosition(1, pb);
        }
    }

    bool IsValidIndex(int i) => i >= 0 && i < nodes.Length;
}
