using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowMouse : MonoBehaviour
{
    public LayerMask _affectedLayer;
    public float _range = 5f;

    public float _circleRadius = 5f; 
    public float _circleHeight = 0.1f;
    public int _circleSegments = 50;

    private LineRenderer lineRenderer;


    void Start()
    {
        // Create a linerenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = _circleSegments + 1;
    }


    void Update()
    {
        // another raycast to draw a circle
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            // Draw a circle on the ground 
            _DrawCircle(_hit.point, Vector3.up, _circleRadius, _circleHeight, _circleSegments);
        }


        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                // Vérifier la distance entre chaque agent et le cercle
                GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");

                foreach (GameObject agentObject in agents)
                {
                    // Get the agent
                    NavMeshAgent agent = agentObject.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        // Take the distance on range to set destination
                        if (Vector3.Distance(hit.point, agentObject.transform.position) <= _range)
                        {
                            // Set destination
                            agent.SetDestination(hit.point);
                        }
                    }
                }
            }
        }
    }

    // Draw the circle on ground
    void _DrawCircle(Vector3 center, Vector3 normal, float radius, float height, int segments)
    {
        Vector3[] points = new Vector3[segments + 1];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

        for (int i = 0; i < segments; i++)
        {
            float angle = i * (360f / segments);
            Vector3 point = center + rotation * Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            points[i] = new Vector3(point.x, center.y + height, point.z);
        }
        points[segments] = points[0]; // Assurer que le cercle est fermé

        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points);
    }
}
