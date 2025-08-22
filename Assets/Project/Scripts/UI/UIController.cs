using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button startButton, stopButton, resetButton;

    void Start()
    {
        startButton.onClick.AddListener(() => SimulationManager.Instance.StartSimulation());
        stopButton.onClick.AddListener(() => SimulationManager.Instance.StopSimulation());
        resetButton.onClick.AddListener(() => SimulationManager.Instance.ResetSimulation());
    }
}
