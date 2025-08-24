using UnityEngine;

public abstract class EditorTool : MonoBehaviour
{
    // Cada herramienta implementa cómo afecta la parte seleccionada
    public abstract void Apply(Transform target);
}