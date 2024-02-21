using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Detectionscript : MonoBehaviour
{
    [Header("Reference")]
    public Transform _PointdeVisé;
    private Transform _agentToChase;
    private NavMeshAgent _navMeshAgent;
    public Patrol _PatrolScript;


    [Header("Parameter")]
    public float _Angle;
    public float _Distance;
    public float _DetectionRadius;
    public Color _RaycastColor;


    [Header("LayerMaskIfNeed")]
    public LayerMask _agentLayer;

    

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Collider[] agentsInRadius = Physics.OverlapSphere(transform.position, _DetectionRadius, _agentLayer);

        if (agentsInRadius.Length > 0)
        {
            foreach (Collider agentCollider in agentsInRadius)
            {
                Vector3 directionToAgent = agentCollider.transform.position - transform.position;
                float angleToAgent = Vector3.Angle(transform.forward, directionToAgent);

                if (angleToAgent <= _Angle)
                {
                    
                    _agentToChase = agentCollider.transform;
                    _navMeshAgent.SetDestination(_agentToChase.position);
                    _PatrolScript.enabled = false;
                    Debug.Log("Agent detected in field of view and within detection radius.");
                    break;
                }
                else
                {
                    _PatrolScript.enabled = true;
                }
            }


        }

        DebugFov(transform, _Angle, _Distance, _RaycastColor);
    }

    private void DebugFov(Transform eyeTransform, float angle, float dist, Color color)
    {
        Vector3 extentLeft = Quaternion.AngleAxis(-angle, eyeTransform.up) * eyeTransform.forward;
        Vector3 extentRight = Quaternion.AngleAxis(angle, eyeTransform.up) * eyeTransform.forward;

        Debug.DrawRay(eyeTransform.position, extentLeft * dist, color);
        Debug.DrawRay(eyeTransform.position, extentRight * dist, color);
        Debug.DrawRay(eyeTransform.position, transform.forward * dist, color);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _DetectionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Agent"))
        {
            Destroy(other.gameObject);
        }
    }

}
