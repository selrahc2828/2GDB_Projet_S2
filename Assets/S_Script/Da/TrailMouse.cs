using UnityEngine;

public class TrailMouse : MonoBehaviour
{
    public LayerMask _terrainLayer;
    public TrailRenderer _trailRendererPrefab;
    public float _verticalOffset = 0.1f;

    private TrailRenderer _currentTrailRenderer;
    private bool _isTracing = false;

    private void Update()
    {
        // Call the starting or finish function
        if (Input.GetMouseButtonDown(0))
        {
            StartTracing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopTracing();
        }

        // Call function to Update the position of trail 
        if (_isTracing)
        {
            UpdateTracerPosition();
        }
    }

    // Start The trail 
    private void StartTracing()
    {
        // Raycast to know where we aim
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            Vector3 trailPosition = hit.point + Vector3.up * _verticalOffset;

            // Instantiate Trail Renderer Prefab 
            GameObject tracerObject = Instantiate(_trailRendererPrefab.gameObject, trailPosition, Quaternion.identity);
            tracerObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Apply rotation
            _currentTrailRenderer = tracerObject.GetComponent<TrailRenderer>();
            _currentTrailRenderer.Clear();
            _isTracing = true;
        }
    }

    private void UpdateTracerPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Update the prefab 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            Vector3 trailPosition = hit.point + Vector3.up * _verticalOffset;

            // Update Trail Renderer position
            _currentTrailRenderer.transform.position = trailPosition;
        }
    }

    private void StopTracing()
    {
        if (_currentTrailRenderer != null)
        {
            // Destroy the prefab 
            _isTracing = false;
            Destroy(_currentTrailRenderer.gameObject, 3f);
        }
    }
}
