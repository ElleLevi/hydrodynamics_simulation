using UnityEngine;
using Obi;

public class VelocityVisualizer : MonoBehaviour
{
    public ObiEmitter emitter;
    public GameObject arrowPrefab;
    public float scaleFactor = 0.1f;

    private GameObject[] arrows;

    void Start()
    {
        arrows = new GameObject[emitter.particleCount];

        for (int i = 0; i < emitter.particleCount; i++)
        {
            arrows[i] = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, transform);
        }
    }

    void LateUpdate()
    {
        var solver = emitter.solver;

        for (int i = 0; i < emitter.particleCount; i++)
        {
            int solverIndex = emitter.solverIndices[i]; // nuevo API

            // Si la partícula es válida:
            if (solverIndex >= 0)
            {
                Vector3 pos = solver.positions[solverIndex];
                Vector3 vel = solver.velocities[solverIndex];

                arrows[i].transform.position = pos;

                if (vel != Vector3.zero)
                    arrows[i].transform.rotation = Quaternion.LookRotation(vel.normalized);

                arrows[i].transform.localScale = Vector3.one * Mathf.Clamp(vel.magnitude * scaleFactor, 0.05f, 0.3f);
            }
        }
    }
}
