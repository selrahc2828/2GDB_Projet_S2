using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMouvement : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Vector3 _randomDestination;
    private float _distanceAgentDestination;
    private float _maxRangeX;
    private float _maxRangeZ;
    private float _minRangeX;
    private float _minRangeZ;
    private static HashSet<Vector3> _occupiedDestinations = new HashSet<Vector3>();
    private const float _destinationRadius = 5f; // Radius to consider a destination as occupied

    // Start is called before the first frame update
    void Start()
    {
        //We determine the coordinate of the Spawn plane of the destination point (could be donner better but i was bored)
        _minRangeZ = -110f;
        _minRangeX = -40f;
        _maxRangeX = 110f;
        _maxRangeZ = 40f;

        //Create the Agent variable
        _agent = GetComponent<NavMeshAgent>();

        //Create 2 variables, ont to later store the exact destination point and the other to use the SamplePosition function
        Vector3 _randomDestinationPoint;
        NavMeshHit hit;

        //Here i create a random point within the spawn plane. Then with SamplePosition i make sure the point is somewhere on the navmesh. if all of that worked, the condition is fulfilled
        if (NavMesh.SamplePosition(new Vector3(Random.Range(_minRangeX, _maxRangeX), -10, Random.Range(_minRangeZ, _maxRangeZ)), out hit, 15f, NavMesh.AllAreas))
        {
            //i store the position of the random location in th randomDestinationPoint variable
            _randomDestinationPoint = hit.position;

            //I set the new destination to the agent
            _agent.SetDestination(_randomDestinationPoint);
        }
    }

    private void Update()
    {
        //This variable is the distance between the agent and his destination
        _distanceAgentDestination = Vector3.Distance(_agent.destination, _agent.transform.position);

        //if the distance is less than 5 units
        if (_distanceAgentDestination <= 5)
        {
            //Create 2 variables, ont to later store the exact destination point and the other to use the SamplePosition function
            Vector3 _randomDestinationPoint;
            NavMeshHit hit;

            //Here i create a random point within the spawn plane. Then with SamplePosition i make sure the point is somewhere on the navmesh. if all of that worked, the condition is fulfilled
            if (NavMesh.SamplePosition(new Vector3(Random.Range(_minRangeX, _maxRangeX), -10, Random.Range(_minRangeZ, _maxRangeZ)), out hit, 15f, NavMesh.AllAreas))
            {
                //i store the position of the random location in th randomDestinationPoint variable
                _randomDestinationPoint = hit.position;

                //I set the new destination to the agent
                _agent.SetDestination(_randomDestinationPoint);
            }
        }
    }
}