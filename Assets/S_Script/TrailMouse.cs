using UnityEngine;

public class TrailMouse : MonoBehaviour
{

    public LayerMask _terrainLayer;
    public TrailRenderer _trailRendererPrefab;

    

    private TrailRenderer _currentTrailRenderer;
    private bool _isTracing = false;

    private void Update()
    {
        // Call the starting or finish fonction
        if (Input.GetMouseButtonDown(0))
        {
            StartTracing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopTracing();
        }

        // Call Fonction to Update the position of trail 
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
            // Instantiate Trail RendererPrefab 
            GameObject tracerObject = new GameObject("TrailRenderer");
            _currentTrailRenderer = tracerObject.AddComponent<TrailRenderer>();
            _currentTrailRenderer = Instantiate(_trailRendererPrefab);
            _currentTrailRenderer.transform.position = hit.point;
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
            _currentTrailRenderer.transform.position = hit.point;
        }



    }

    private void StopTracing()
    {
        if (_currentTrailRenderer != null)
        {
            // Destroy the prefab 
            _isTracing = false;
            //Destroy(_currentTrailRenderer.gameObject);
        }
    }
}
