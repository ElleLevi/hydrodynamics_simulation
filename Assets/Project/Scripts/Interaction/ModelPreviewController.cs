using UnityEngine;
using UnityEngine.EventSystems;

public class ModelPreviewController : MonoBehaviour, IDragHandler, IScrollHandler
{
    public Transform targetModel; // El modelo importado
    public float rotationSpeed = 5f;
    public float zoomSpeed = 5f;
    public Camera previewCamera;

    public void OnDrag(PointerEventData eventData)
    {
        if (targetModel != null)
        {
            float rotX = eventData.delta.y * rotationSpeed;
            float rotY = -eventData.delta.x * rotationSpeed;
            targetModel.Rotate(Vector3.up, rotY, Space.World);
            targetModel.Rotate(Vector3.right, rotX, Space.World);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (previewCamera != null)
        {
            previewCamera.fieldOfView -= eventData.scrollDelta.y * zoomSpeed;
            previewCamera.fieldOfView = Mathf.Clamp(previewCamera.fieldOfView, 20, 80);
        }
    }
}
