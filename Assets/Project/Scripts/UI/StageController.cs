using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject importPanel;
    public GameObject fluidPanel;
    public GameObject simulationPanel;

    private int currentStage = 0;

    private void Start()
    {
        ShowStage(0); // Empezar en etapa 0: importación
    }

    public void ShowStage(int stage)
    {
        currentStage = stage;

        importPanel.SetActive(stage == 0);
        fluidPanel.SetActive(stage == 1);
        simulationPanel.SetActive(stage == 2);
    }

    public void NextStage()
    {
        ShowStage(currentStage + 1);
    }

    public void PreviousStage()
    {
        ShowStage(currentStage - 1);
    }
}
