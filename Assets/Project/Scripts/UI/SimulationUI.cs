using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour
{
    [Header("Toggles")]
    public Toggle toggleVectores;
    public Toggle toggleLineas;
    public Toggle togglePresion;

    private void Start()
    {
        toggleVectores.onValueChanged.AddListener(OnToggleVectores);
        toggleLineas.onValueChanged.AddListener(OnToggleLineas);
        togglePresion.onValueChanged.AddListener(OnTogglePresion);
    }

    private void OnToggleVectores(bool active)
    {
        VisualManager.Instance.ShowVectors(active);
    }

    private void OnToggleLineas(bool active)
    {
        VisualManager.Instance.ShowFlowLines(active);
    }

    private void OnTogglePresion(bool active)
    {
        VisualManager.Instance.ShowPressureMap(active);
    }
}
