using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] public Transform target;

    [Header("Rotation")]
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float distanceFromTarget = 20.0f;
    [SerializeField] public float rotationSmoothTime = 0.12f;

    [Header("RotationClamp")]
    //[SerializeField] public float minRotationY = 0f;
    //[SerializeField] public float maxRotationY = 180f;
    [SerializeField] public Vector2 pitchMinMax = new Vector2(-40, 85);

    Vector3 currentRotation;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }

        Vector3 desiredPosition = target.position - transform.forward * distanceFromTarget;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, rotationSmoothTime);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentRotation.y += mouseX * rotationSpeed;
        currentRotation.x -= mouseY * rotationSpeed;
        currentRotation.x = Mathf.Clamp(currentRotation.x, pitchMinMax.x, pitchMinMax.y);

        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }
}
