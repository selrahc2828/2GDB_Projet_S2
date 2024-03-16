using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] public Transform target;

    [Header("Rotation")]
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float distanceFromTarget;
    [SerializeField] public float rotationSmoothTime;

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
