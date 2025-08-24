using UnityEngine;

public class RotateTool : EditorTool
{
    public float rotationSpeed = 100f;

    public override void Apply(Transform target)
    {
        float rotY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float rotX = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        target.Rotate(Vector3.up, rotY, Space.World);
        target.Rotate(Vector3.right, rotX, Space.World);
    }
}
