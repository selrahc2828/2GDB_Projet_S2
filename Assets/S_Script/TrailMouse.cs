using UnityEngine;

public class TrailMouse : MonoBehaviour
{
    public LayerMask terrainLayer;
    public TrailRenderer trailRendererPrefab;

    private TrailRenderer currentTrailRenderer;
    private bool isTracing = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartTracing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopTracing();
        }

        if (isTracing)
        {
            UpdateTracerPosition();
        }
    }

    private void StartTracing()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            GameObject tracerObject = new GameObject("TrailRenderer");
            currentTrailRenderer = tracerObject.AddComponent<TrailRenderer>();
            currentTrailRenderer = Instantiate(trailRendererPrefab);
            currentTrailRenderer.transform.position = hit.point;
            currentTrailRenderer.Clear();
            isTracing = true;
        }
    }

    private void UpdateTracerPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            currentTrailRenderer.transform.position = hit.point;
        }
    }

    private void StopTracing()
    {
        if (currentTrailRenderer != null)
        {
            isTracing = false;
            Destroy(currentTrailRenderer.gameObject);
        }
    }
}
