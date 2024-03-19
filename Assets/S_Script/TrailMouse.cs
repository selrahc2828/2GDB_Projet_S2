using UnityEngine;

public class TrailMouse : MonoBehaviour
{
    public LayerMask _terrainLayer;
    public GameObject _lineRendererPrefab; 

    private LineRenderer _currentLineRenderer;
    private bool _isTracing = false;
    public GameManager _GameManagerScript;
    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }

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
