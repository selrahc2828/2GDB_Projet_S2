using UnityEngine;

public class TrailMouse : MonoBehaviour
{
    public LayerMask _terrainLayer;
    public GameObject _lineRendererPrefab; // Reference to the GameObject with LineRenderer attached

    private LineRenderer _currentLineRenderer;
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

        // Call Function to Update the position of line
        if (_isTracing)
        {
            UpdateTracerPosition();
        }
    }

    // Start The line 
    private void StartTracing()
    {
        // Raycast to know where we aim
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            // Instantiate LineRendererPrefab 
            GameObject lineObject = Instantiate(_lineRendererPrefab);
            _currentLineRenderer = lineObject.GetComponent<LineRenderer>();
            _currentLineRenderer.positionCount = 1; // Set initial position count
            _currentLineRenderer.SetPosition(0, hit.point); // Set initial position
            _isTracing = true;
        }
    }


    private void UpdateTracerPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Update the line renderer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            Vector3 newPosition = hit.point;

            // Calculate the rotation to align with the Y axis
            Quaternion rotation = Quaternion.LookRotation(Vector3.up);

            // Set the rotation
            _currentLineRenderer.transform.rotation = rotation;

            // Adjust the position to be slightly above the ground to avoid z-fighting
            newPosition.y += 0.01f;

            // Set the position
            _currentLineRenderer.positionCount++;
            _currentLineRenderer.SetPosition(_currentLineRenderer.positionCount - 1, newPosition);
        }
    }


    private void StopTracing()
    {
        _isTracing = false;
        _currentLineRenderer = null; // Reset the current LineRenderer reference
    }
}
