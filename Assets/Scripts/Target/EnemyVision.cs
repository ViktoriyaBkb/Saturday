using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EnemyVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float ViewDistance = 15f;
    public float ViewAngle = 60f;
    public LayerMask ObstacleMask;
    public LayerMask TargetMask;

    [Header("Visual")]
    public Color ConeColor = new Color(1f, 0f, 0f, 0.35f);

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Material _material;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh();
        _mesh.name = "VisionCone";
        _meshFilter.mesh = _mesh;

        _material = new Material(Shader.Find("Sprites/Default"));
        _material.color = ConeColor;
        _meshRenderer.material = _material;

        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;
    }

    void Update()
    {
        //Debug.Log($"[{name}] Update is running");
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            _meshRenderer.enabled = false;
            return;
        }
        _meshRenderer.enabled = true;

        DrawVisionCone();
        CheckForCorpses();
    }

    private void DrawVisionCone()
    {
        Vector3 leftDir = Quaternion.Euler(0, -ViewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, ViewAngle / 2, 0) * transform.forward;

        Vector3[] vertices = new Vector3[3];
        vertices[0] = Vector3.zero;
        vertices[1] = transform.InverseTransformDirection(leftDir) * ViewDistance;
        vertices[2] = transform.InverseTransformDirection(rightDir) * ViewDistance;

        int[] triangles = new int[] { 0, 1, 2 };

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private void CheckForCorpses()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver) return;

        Collider[] corpsesInRange = Physics.OverlapSphere(transform.position, ViewDistance, TargetMask);

        foreach (var corpse in corpsesInRange)
        {
            if (corpse.transform == transform) continue;

            Vector3 dirToCorpse = (corpse.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToCorpse);

            if (angle < ViewAngle / 2)
            {
                if (!Physics.Linecast(transform.position, corpse.transform.position, ObstacleMask))
                {
                    Debug.Log($"Enemy {name} spotted corpse {corpse.name}!");
                    GameManager.Instance.OnTargetKilled(false);
                    break;
                }
            }
        }
    }
}