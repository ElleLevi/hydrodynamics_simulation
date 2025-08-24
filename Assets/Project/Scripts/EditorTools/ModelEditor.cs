using UnityEditor.EditorTools;
using UnityEngine;

public class ModelEditor : MonoBehaviour
{
    public Transform selectedPart; // La parte del modelo que estamos editando

    private EditorTool currentTool; // Esta es la clase que creaste tú, no la de UnityEditor

    public void SelectPart(Transform part)
    {
        selectedPart = part;
    }

    public void SetTool(EditorTool tool)
    {
        currentTool = tool;
    }

    void Update()
    {
        if (selectedPart != null && currentTool != null)
        {
            currentTool.Apply(selectedPart);
        }
    }
}
