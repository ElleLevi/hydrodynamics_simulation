using UnityEngine;
using Obi;
using System.Collections.Generic;

public class VelocityVisualizer : MonoBehaviour
{
    public ObiEmitter emitter;
    public GameObject arrowPrefab;
    public float scaleFactor = 0.1f;

    // Diccionario: indice de particula -> flecha asociada
    private Dictionary<int, GameObject> arrowMap = new Dictionary<int, GameObject>();

    void LateUpdate()
    {
        var solver = emitter.solver;

        for (int i = 0; i < emitter.particleCount; i++)
        {
            int solverIndex = emitter.solverIndices[i];

            // Particula activa
            if (solverIndex >= 0)
            {
                Vector3 pos = solver.positions[solverIndex];
                Vector3 vel = solver.velocities[solverIndex];

                // Si no existe flecha para esta particula -> crearla
                if (!arrowMap.ContainsKey(i))
                {
                    GameObject arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, transform);
                    arrowMap[i] = arrow;
                }

                GameObject currentArrow = arrowMap[i];
                currentArrow.transform.position = pos;

                if (vel != Vector3.zero)
                    currentArrow.transform.rotation = Quaternion.LookRotation(vel.normalized);

                currentArrow.transform.localScale = Vector3.one * Mathf.Clamp(vel.magnitude * scaleFactor, 0.05f, 0.3f);
            }
            else
            {
                // Particula desaparecida -> destruir flecha si existia
                if (arrowMap.ContainsKey(i))
                {
                    Destroy(arrowMap[i]);
                    arrowMap.Remove(i);
                }
            }
        }
    }
}
