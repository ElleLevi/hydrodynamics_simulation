using Obi;
using System.Collections.Generic;
using UnityEngine;

public class FlowLinesVisualizer : MonoBehaviour
{
    [Header("Emisor y Material")]
    public ObiEmitter emitter;
    public Material lineMaterial;

    [Header("Líneas de flujo")]
    public int maxLines = 50;
    public int maxPoints = 100;
    public float width = 0.01f;

    [Header("Color por 'presión' (densidad proxy)")]
    public Gradient densityColorRamp;
    public bool autoFitDensityRange = true;
    public float minDensity = 900f;
    public float maxDensity = 1100f;

    private struct LineData
    {
        public LineRenderer lr;
        public Queue<Vector3> positions;
        public Queue<float> densities;
    }

    private readonly Dictionary<int, LineData> lines = new Dictionary<int, LineData>();
    private float observedMin = float.PositiveInfinity;
    private float observedMax = float.NegativeInfinity;

    void Awake()
    {
        if (densityColorRamp == null || densityColorRamp.colorKeys.Length == 0)
        {
            densityColorRamp = new Gradient();
            densityColorRamp.SetKeys(
                new[] {
                    new GradientColorKey(Color.blue, 0f),
                    new GradientColorKey(Color.cyan, 0.33f),
                    new GradientColorKey(Color.yellow, 0.66f),
                    new GradientColorKey(Color.red, 1f)
                },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );
        }
    }

    void LateUpdate()
    {
        if (emitter == null || emitter.solver == null)
            return;

        var solver = emitter.solver;
        int particleCount = emitter.particleCount;
        int linesToDraw = Mathf.Min(maxLines, particleCount);

        HashSet<int> touched = new HashSet<int>();

        for (int i = 0; i < linesToDraw; i++)
        {
            int solverIndex = emitter.solverIndices[i];

            if (solverIndex < 0)
            {
                if (lines.TryGetValue(i, out var dead))
                {
                    if (dead.lr != null)
                    {
                        dead.lr.positionCount = 0;
                        Destroy(dead.lr.gameObject);
                    }
                    lines.Remove(i);
                }
                continue;
            }

            if (!lines.TryGetValue(i, out var ld))
            {
                ld = new LineData
                {
                    lr = CreateLineRenderer(i),
                    positions = new Queue<Vector3>(maxPoints + 1),
                    densities = new Queue<float>(maxPoints + 1)
                };
            }

            Vector3 pos = (solver.renderablePositions != null && solver.renderablePositions.count > 0)
                          ? solver.renderablePositions[solverIndex]
                          : solver.positions[solverIndex];

            float density = solver.fluidData[solverIndex].x;

            if (autoFitDensityRange)
            {
                if (density < observedMin) observedMin = density;
                if (density > observedMax) observedMax = density;
            }

            ld.positions.Enqueue(pos);
            ld.densities.Enqueue(density);

            while (ld.positions.Count > maxPoints)
            {
                ld.positions.Dequeue();
                ld.densities.Dequeue();
            }

            var posArray = ld.positions.ToArray();
            ld.lr.positionCount = posArray.Length;
            ld.lr.SetPositions(posArray);

            // Gradiente optimizado para máximo 8 keys
            var densArray = ld.densities.ToArray();
            int n = densArray.Length;
            if (n > 0)
            {
                int keyCount = Mathf.Min(n, 8); // máximo 8 keys
                var cks = new GradientColorKey[keyCount];
                var aks = new GradientAlphaKey[keyCount];

                for (int k = 0; k < keyCount; ++k)
                {
                    int index = (keyCount == 1) ? 0 : Mathf.RoundToInt((float)k / (keyCount - 1) * (n - 1));
                    float tLine = (keyCount == 1) ? 0f : (float)k / (keyCount - 1);
                    float tDensity = Mathf.InverseLerp(autoFitDensityRange ? observedMin : minDensity,
                                                       autoFitDensityRange ? observedMax : maxDensity,
                                                       densArray[index]);
                    Color c = densityColorRamp.Evaluate(tDensity);

                    cks[k] = new GradientColorKey(c, tLine);
                    aks[k] = new GradientAlphaKey(c.a, tLine);
                }

                var g = new Gradient();
                g.SetKeys(cks, aks);
                ld.lr.colorGradient = g;
            }

            lines[i] = ld;
            touched.Add(i);
        }

        // Eliminar líneas sobrantes
        var toRemove = new List<int>();
        foreach (var kv in lines)
        {
            if (!touched.Contains(kv.Key))
            {
                if (kv.Value.lr != null)
                {
                    kv.Value.lr.positionCount = 0;
                    Destroy(kv.Value.lr.gameObject);
                }
                toRemove.Add(kv.Key);
            }
        }
        foreach (var key in toRemove) lines.Remove(key);
    }

    private LineRenderer CreateLineRenderer(int index)
    {
        var go = new GameObject($"FlowLine_{index}");
        go.transform.SetParent(transform, false);

        var lr = go.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.widthMultiplier = width;
        lr.positionCount = 0;
        lr.useWorldSpace = true;
        lr.numCornerVertices = 2;
        lr.numCapVertices = 2;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;

        return lr;
    }
}
