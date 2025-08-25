using Obi;
using System.Collections.Generic;
using UnityEngine;

public class FlowLinesVisualizer : MonoBehaviour
{
    public enum ColorMode { Density, Velocity }

    [Header("Emisor y Material")]
    public ObiEmitter emitter;
    public Material lineMaterial;

    [Header("Líneas de flujo")]
    public int maxLines = 50;
    public int maxPoints = 100;
    public float width = 0.01f;

    [Header("Coloreo")]
    public ColorMode colorMode = ColorMode.Density;

    [Tooltip("Colores por densidad (proxy de presión)")]
    public Gradient densityColorRamp;
    public bool autoFitDensityRange = true;
    public float minDensity = 900f;
    public float maxDensity = 1100f;

    [Tooltip("Colores por velocidad")]
    public Gradient velocityColorRamp;
    public float maxSpeed = 5f;

    private struct LineData
    {
        public LineRenderer lr;
        public Queue<Vector3> positions;
        public Queue<float> values; // densidad o velocidad segun modo
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

        if (velocityColorRamp == null || velocityColorRamp.colorKeys.Length == 0)
        {
            velocityColorRamp = new Gradient();
            velocityColorRamp.SetKeys(
                new[] {
                    new GradientColorKey(Color.green, 0f),
                    new GradientColorKey(Color.yellow, 0.5f),
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
                    values = new Queue<float>(maxPoints + 1)
                };
            }

            Vector3 pos = (solver.renderablePositions != null && solver.renderablePositions.count > 0)
                          ? solver.renderablePositions[solverIndex]
                          : solver.positions[solverIndex];

            float value = 0f;
            switch (colorMode)
            {
                case ColorMode.Density:
                    float density = solver.fluidData[solverIndex].x;
                    if (autoFitDensityRange)
                    {
                        if (density < observedMin) observedMin = density;
                        if (density > observedMax) observedMax = density;
                    }
                    value = density;
                    break;

                case ColorMode.Velocity:
                    Vector3 vel = solver.velocities[solverIndex];
                    value = vel.magnitude;
                    break;
            }

            ld.positions.Enqueue(pos);
            ld.values.Enqueue(value);

            while (ld.positions.Count > maxPoints)
            {
                ld.positions.Dequeue();
                ld.values.Dequeue();
            }

            var posArray = ld.positions.ToArray();
            var valArray = ld.values.ToArray();
            int n = valArray.Length;

            ld.lr.positionCount = posArray.Length;
            ld.lr.SetPositions(posArray);

            if (n > 0)
            {
                int keyCount = Mathf.Min(8, n); // máximo 8 keys
                var cks = new GradientColorKey[keyCount];
                var aks = new GradientAlphaKey[keyCount];

                for (int k = 0; k < keyCount; ++k)
                {
                    int index;
                    if (keyCount == 1 || n == 1)
                        index = 0;
                    else
                        index = Mathf.Clamp(Mathf.RoundToInt((float)k / (keyCount - 1) * (n - 1)), 0, n - 1);

                    float tLine = (keyCount == 1) ? 0f : (float)k / (keyCount - 1);

                    float tValue = 0f;
                    if (colorMode == ColorMode.Density)
                    {
                        tValue = Mathf.InverseLerp(autoFitDensityRange ? observedMin : minDensity,
                                                   autoFitDensityRange ? observedMax : maxDensity,
                                                   valArray[index]);
                        cks[k] = new GradientColorKey(densityColorRamp.Evaluate(tValue), tLine);
                        aks[k] = new GradientAlphaKey(1f, tLine);
                    }
                    else
                    {
                        tValue = Mathf.Clamp01(valArray[index] / maxSpeed);
                        cks[k] = new GradientColorKey(velocityColorRamp.Evaluate(tValue), tLine);
                        aks[k] = new GradientAlphaKey(1f, tLine);
                    }
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
        lr.material = lineMaterial; // sprite/default soporta gradiente
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
