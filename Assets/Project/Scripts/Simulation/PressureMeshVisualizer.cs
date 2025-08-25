using UnityEngine;
using Obi;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PressureMeshVisualizer : MonoBehaviour
{
    [Header("Obi Emitter")]
    public ObiEmitter emitter;

    [Header("Gradient de densidad")]
    public Gradient densityColorRamp;

    [Header("Parámetros")]
    public float radius = 0.1f; // radio de influencia de cada partícula
    public bool autoFit = true;
    public float minDensity = 900f;
    public float maxDensity = 1100f;

    private Mesh mesh;
    private Color[] vertexColors;
    private Vector3[] vertices;
    private float observedMin = float.PositiveInfinity;
    private float observedMax = float.NegativeInfinity;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        vertexColors = new Color[vertices.Length];

        if (densityColorRamp == null || densityColorRamp.colorKeys.Length == 0)
        {
            densityColorRamp = new Gradient();
            densityColorRamp.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.blue, 0f),
                    new GradientColorKey(Color.cyan, 0.33f),
                    new GradientColorKey(Color.yellow, 0.66f),
                    new GradientColorKey(Color.red, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
        }
    }

    void LateUpdate()
    {
        if (emitter == null || emitter.solver == null) return;
        var solver = emitter.solver;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            float sumDensity = 0f;
            int count = 0;

            for (int j = 0; j < emitter.particleCount; j++)
            {
                int solverIndex = emitter.solverIndices[j];
                if (solverIndex < 0) continue;

                Vector3 particlePos = solver.positions[solverIndex];
                float density = solver.fluidData[solverIndex].x;

                float dist = Vector3.Distance(worldPos, particlePos);
                if (dist <= radius)
                {
                    sumDensity += density;
                    count++;
                    if (autoFit)
                    {
                        if (density < observedMin) observedMin = density;
                        if (density > observedMax) observedMax = density;
                    }
                }
            }

            float avgDensity = (count > 0) ? sumDensity / count : minDensity;

            // Calculamos t y prevenimos NaN o Infinity
            float t = Mathf.InverseLerp(autoFit ? observedMin : minDensity,
                                        autoFit ? observedMax : maxDensity,
                                        avgDensity);
            if (float.IsNaN(t) || float.IsInfinity(t))
                t = 0f;
            t = Mathf.Clamp01(t);

            vertexColors[i] = densityColorRamp.Evaluate(t);
        }

        mesh.colors = vertexColors;
    }
}
