using UnityEngine;

public class TrailElement : MonoBehaviour
{
    public float _rotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}
