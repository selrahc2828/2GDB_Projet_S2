using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMouvement : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Vector3 _randomDestination;
    private float _maxRangeX;
    private float _maxRangeZ;
    private float _minRangeX;
    private float _minRangeZ;
    private static HashSet<Vector3> _occupiedDestinations = new HashSet<Vector3>();
    private const float _destinationRadius = 5f; // Radius to consider a destination as occupied

    // Start is called before the first frame update
    void Start()
    {
        _minRangeZ = -291f;
        _minRangeX = -291f;
        _maxRangeX = 291f;
        _maxRangeZ = 291f;
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(AgentDestination());
    }

    IEnumerator AgentDestination()
    {
        while (true)
        {
            Vector3 _randomDestinationPoint;
            do
            {
                _randomDestinationPoint = new Vector3(Random.Range(_minRangeX, _maxRangeX), 640, Random.Range(_minRangeZ, _maxRangeZ));
            } while (IsDestinationOccupied(_randomDestinationPoint));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(_randomDestinationPoint, out hit, 30, NavMesh.AllAreas))
            {
                _randomDestination = hit.position;
                _occupiedDestinations.Add(_randomDestination);
            }

            _agent.SetDestination(_randomDestination);
            yield return new WaitForSeconds(1f);
        }
    }

    bool IsDestinationOccupied(Vector3 destination)
    {
        foreach (var occupiedDestination in _occupiedDestinations)
        {
            if ((destination - occupiedDestination).sqrMagnitude <= _destinationRadius * _destinationRadius)
            {
                return true;
            }
        }
        return false;
    }
}
