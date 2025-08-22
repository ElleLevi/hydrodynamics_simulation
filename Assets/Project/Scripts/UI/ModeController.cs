using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    public GameObject fluidSystem;
    public GameObject emitterHandle;
    public GameObject modelHandle;
    public Button startSimulationButton;

    private void Start()
    {
        // Arrancar en modo edición
        fluidSystem.SetActive(false);
        emitterHandle.SetActive(false);
        modelHandle.SetActive(false);

        startSimulationButton.onClick.AddListener(ActivateSimulation);
    }

    public void EnableEditMode(GameObject model)
    {
        modelHandle.SetActive(true);
        modelHandle.transform.SetParent(model.transform);
        modelHandle.transform.localPosition = Vector3.zero;

        emitterHandle.SetActive(true);
    }

    public void ActivateSimulation()
    {
        fluidSystem.SetActive(true);
        modelHandle.SetActive(false);
        emitterHandle.SetActive(false);
    }
}
