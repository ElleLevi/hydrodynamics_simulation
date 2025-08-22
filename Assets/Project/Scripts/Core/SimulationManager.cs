using UnityEngine;
using Obi;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    public ObiSolver solver;
    public ObiEmitter emitter;
    public ObiFixedUpdater updater;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        if (solver != null && updater != null && !updater.solvers.Contains(solver))
        {
            updater.solvers.Add(solver);
        }
    }

    public void StartSimulation()
    {
        if (emitter != null)
            emitter.gameObject.SetActive(true);

        if (solver != null)
            solver.gameObject.SetActive(true);
    }

    public void StopSimulation()
    {
        if (emitter != null)
            emitter.gameObject.SetActive(false);
    }

    public void ResetSimulation()
    {
        if (emitter == null) return;

        // Paso 1: desactivar el emisor para detener nuevas partículas
        emitter.enabled = false;

        // Paso 2: eliminar todas las partículas activas
        emitter.KillAll();

        // Paso 3: reactivar el emisor
        emitter.enabled = true;
    }

}
