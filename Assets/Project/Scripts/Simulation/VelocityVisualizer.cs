using UnityEngine;
using Obi;
using System.Collections.Generic;

public class VelocityVisualizer : MonoBehaviour
{
    public ObiEmitter emitter;
    public GameObject arrowPrefab;

    [Header("Escalado por velocidad")]
    public float scaleFactor = 0.1f;
    public float maxSpeed = 5f;

    [Header("Color según velocidad")]
    public Gradient velocityGradient;

    [Header("Tangencia a la partícula")]
    public float particleRadius = 0.05f;
    public float arrowLengthFactor = 1.0f;

    [Header("Muestreo")]
    [Range(1, 2000)]
    public int arrowCount = 50;

    private readonly List<GameObject> arrows = new List<GameObject>();

    void LateUpdate()
    {
        if (emitter == null || emitter.solver == null) return;
        var solver = emitter.solver;

        int particleCount = emitter.particleCount;
        if (particleCount <= 0) return;

        int count = Mathf.Clamp(arrowCount, 1, particleCount);

        // Asegurar cantidad de flechas instanciadas
        while (arrows.Count < count)
        {
            var go = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, transform);
            arrows.Add(go);
        }
        while (arrows.Count > count)
        {
            var last = arrows[arrows.Count - 1];
            if (last) Destroy(last);
            arrows.RemoveAt(arrows.Count - 1);
        }

        int groupSize = Mathf.CeilToInt((float)particleCount / count);

        for (int a = 0; a < count; a++)
        {
            int start = a * groupSize;
            int end = Mathf.Min(start + groupSize, particleCount);

            Vector3 weightedPos = Vector3.zero;
            Vector3 weightedVel = Vector3.zero;
            float totalWeight = 0f;
            int valid = 0;

            for (int i = start; i < end; i++)
            {
                int solverIndex = emitter.solverIndices[i];
                if (solverIndex < 0) continue;

                // Vector4 -> Vector3
                Vector4 p4 = (solver.renderablePositions != null && solver.renderablePositions.count > 0) ?
                             solver.renderablePositions[solverIndex] : solver.positions[solverIndex];
                Vector3 pos3 = new Vector3(p4.x, p4.y, p4.z);

                Vector4 v4 = solver.velocities[solverIndex];
                Vector3 vel3 = new Vector3(v4.x, v4.y, v4.z);

                float speed = vel3.magnitude;
                if (speed < 1e-6f) continue; // ignorar partículas casi estáticas

                weightedPos += pos3 * speed;   // ponderar posición por velocidad
                weightedVel += vel3 * speed;   // ponderar dirección por velocidad
                totalWeight += speed;
                valid++;
            }

            var arrow = arrows[a];
            if (!arrow) continue;

            if (valid > 0 && totalWeight > 1e-6f)
            {
                arrow.SetActive(true);

                Vector3 avgPos = weightedPos / totalWeight;
                Vector3 avgVel = weightedVel / totalWeight;
                float speed = avgVel.magnitude;
                Vector3 dir = avgVel / speed;

                float arrowLength = Mathf.Clamp(speed * scaleFactor, 0.05f, 0.3f) * arrowLengthFactor;

                Vector3 baseOffset = dir * (particleRadius + arrowLength * 0.5f);
                arrow.transform.position = avgPos + baseOffset;
                arrow.transform.rotation = Quaternion.LookRotation(dir);
                arrow.transform.localScale = Vector3.one * arrowLength;

                float t = Mathf.Clamp01(speed / maxSpeed);
                Color c = velocityGradient != null ? velocityGradient.Evaluate(t) : Color.Lerp(Color.green, Color.red, t);
                var rend = arrow.GetComponentInChildren<Renderer>();
                if (rend) rend.material.color = c;
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }
}
