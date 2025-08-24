using UnityEngine;

public class ScaleTool : EditorTool
{
    public float scaleSpeed = 0.1f;

    public override void Apply(Transform target)
    {
        float scaleChange = Input.GetAxis("Vertical") * scaleSpeed;
        target.localScale += Vector3.one * scaleChange;
    }
}
