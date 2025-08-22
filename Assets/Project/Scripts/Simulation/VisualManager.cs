// Scripts/Simulation/VisualManager.cs
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public static VisualManager Instance { get; private set; }

    public GameObject vectorsVisualizer;
    public GameObject flowLinesVisualizer;
    public GameObject pressureMapVisualizer;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void ShowVectors(bool active)
    {
        if (vectorsVisualizer != null)
            vectorsVisualizer.SetActive(active);
    }

    public void ShowFlowLines(bool active)
    {
        if (flowLinesVisualizer != null)
            flowLinesVisualizer.SetActive(active);
    }

    public void ShowPressureMap(bool active)
    {
        if (pressureMapVisualizer != null)
            pressureMapVisualizer.SetActive(active);
    }
}
