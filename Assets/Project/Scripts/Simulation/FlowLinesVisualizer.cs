using UnityEngine;
using Obi;
using System.Collections.Generic;

public class FlowLinesVisualizer : MonoBehaviour
{
    public ObiEmitter emitter;
    public Material lineMaterial;
    public int maxPoints = 100;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private List<Queue<Vector3>> points = new List<Queue<Vector3>>();

    void Start()
    {
        for (int i = 0; i < emitter.particleCount; i++)
        {
            GameObject lineObj = new GameObject($"Line_{i}");
            lineObj.transform.SetParent(transform);

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 0;
            lr.material = lineMaterial;
            lr.widthMultiplier = 0.01f;

            lines.Add(lr);
            points.Add(new Queue<Vector3>());
        }
    }

    void LateUpdate()
    {
        var solver = emitter.solver;

        for (int i = 0; i < emitter.particleCount; i++)
        {
            int solverIndex = emitter.solverIndices[i]; // nuevo API

            if (solverIndex >= 0)
            {
                // Añadir la posición al historial
                points[i].Enqueue(solver.positions[solverIndex]);

                if (points[i].Count > maxPoints)
                    points[i].Dequeue();

                // Actualizar el LineRenderer
                lines[i].positionCount = points[i].Count;
                lines[i].SetPositions(points[i].ToArray());
            }
        }
    }
}
