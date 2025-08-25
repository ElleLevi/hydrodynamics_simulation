using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubdividedPlaneMesh : MonoBehaviour
{
    [Header("Tamaño del plano")]
    public float width = 1f;
    public float length = 1f;

    [Header("Subdivisiones")]
    public int widthSegments = 10;
    public int lengthSegments = 10;

    [Header("Material")]
    public Material material;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = material;

        mf.mesh = GenerateMesh(width, length, widthSegments, lengthSegments);
    }

    private Mesh GenerateMesh(float width, float length, int wSeg, int lSeg)
    {
        Mesh mesh = new Mesh();
        mesh.name = "SubdividedPlane";

        int vertCountX = wSeg + 1;
        int vertCountZ = lSeg + 1;

        Vector3[] vertices = new Vector3[vertCountX * vertCountZ];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[wSeg * lSeg * 6];

        float dx = width / wSeg;
        float dz = length / lSeg;

        // Crear vértices y UVs
        for (int z = 0; z < vertCountZ; z++)
        {
            for (int x = 0; x < vertCountX; x++)
            {
                int idx = z * vertCountX + x;
                float vx = x * dx - width / 2f;
                float vz = z * dz - length / 2f;
                vertices[idx] = new Vector3(vx, 0f, vz);
                uv[idx] = new Vector2((float)x / wSeg, (float)z / lSeg);
            }
        }

        // Crear triángulos
        int tri = 0;
        for (int z = 0; z < lSeg; z++)
        {
            for (int x = 0; x < wSeg; x++)
            {
                int topLeft = z * vertCountX + x;
                int bottomLeft = (z + 1) * vertCountX + x;
                int topRight = topLeft + 1;
                int bottomRight = bottomLeft + 1;

                // Primer triángulo
                triangles[tri++] = topLeft;
                triangles[tri++] = bottomLeft;
                triangles[tri++] = topRight;

                // Segundo triángulo
                triangles[tri++] = topRight;
                triangles[tri++] = bottomLeft;
                triangles[tri++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
