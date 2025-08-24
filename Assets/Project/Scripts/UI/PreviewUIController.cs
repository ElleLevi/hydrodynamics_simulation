using UnityEngine;
using UnityEngine.UI;

public class PreviewUIController : MonoBehaviour
{
    [Header("Modelo y Editor")]
    public GameObject previewModel;          // Modelo 3D a mostrar
    public ModelEditor modelEditor;          // Script que maneja la edición del modelo

    [Header("Botones de Modo")]
    public Button editButton;
    public Button simulateButton;

    [Header("Botones de Herramientas")]
    public Button moveToolButton;
    public Button rotateToolButton;
    public Button scaleToolButton;

    [Header("Herramientas")]
    public EditorTool moveTool;
    public EditorTool rotateTool;
    public EditorTool scaleTool;

    private bool isEditing = false;

    void Start()
    {
        // Modo edición / simulación
        editButton.onClick.AddListener(ToggleEditMode);
        simulateButton.onClick.AddListener(StartSimulation);

        // Herramientas
        moveToolButton.onClick.AddListener(() => modelEditor.SetTool(moveTool));
        rotateToolButton.onClick.AddListener(() => modelEditor.SetTool(rotateTool));
        scaleToolButton.onClick.AddListener(() => modelEditor.SetTool(scaleTool));

        SetEditMode(false);
    }

    void ToggleEditMode()
    {
        SetEditMode(!isEditing);
    }

    void SetEditMode(bool enable)
    {
        isEditing = enable;

        if (modelEditor != null)
            modelEditor.enabled = enable;

        // Cambiar apariencia de botones para indicar estado
        editButton.GetComponent<Image>().color = enable ? Color.green : Color.white;

        // Opcional: desactivar la simulación mientras se edita
        // fluidSystem.SetActive(!enable);
    }

    void StartSimulation()
    {
        SetEditMode(false);
        Debug.Log("Simulación iniciada");
        // Aquí se activa el sistema de fluidos
        // fluidSystem.SetActive(true);
    }
}
