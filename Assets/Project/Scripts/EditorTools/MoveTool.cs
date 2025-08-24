using UnityEditor.EditorTools;
using UnityEngine;

public class MoveTool : EditorTool
{
    public float speed = 0.1f;

    public override void Apply(Transform target)
    {
        float x = Input.GetAxis("Horizontal") * speed;
        float y = Input.GetAxis("Mouse ScrollWheel") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        target.Translate(x, y, z, Space.World);
    }
}
