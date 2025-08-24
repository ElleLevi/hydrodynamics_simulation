using UnityEngine;

public abstract class EditorTool : MonoBehaviour
{
    // Cada herramienta implementa c�mo afecta la parte seleccionada
    public abstract void Apply(Transform target);
}