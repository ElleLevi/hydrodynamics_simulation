using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float up = 0;

        if (Input.GetKey(KeyCode.E)) up = 1;
        if (Input.GetKey(KeyCode.Q)) up = -1;

        Vector3 dir = new Vector3(h, up, v);
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        if (Input.GetMouseButton(1)) // Click derecho para mirar
        {
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.left, mouseY, Space.Self);
        }
    }
}
