using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpawner : MonoBehaviour
{
    [Header("r�f�rence")]
    public GameObject agentPrefab;
    public GameObject _CubeToLife;

    [Header("Param�tre")]
    public float _NumberOfCollision;
    public float _MaxTimeBeforeDeath;
    public float _TimeBeforeDeath;
    private float _Time;
    public float maxOffsetDistance = 1f;

    private bool hasSpawned = false; 

    private int collisionCount = 0;

    private void Start()
    {
        _TimeBeforeDeath = _MaxTimeBeforeDeath;
    }


    private void Update()
    {
        // Add second for the timer
        _Time += Time.deltaTime;

        if (_Time >= _TimeBeforeDeath)
        {
            Vector3 deathPosition = transform.position;
            // destroy the GameObject if the timer is > than our value 
            Destroy(gameObject);
            Instantiate(_CubeToLife, deathPosition, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            // Up count of collision
            collisionCount++;
        }
        

        if (collisionCount >= _NumberOfCollision)
        {
            Vector3 deathPosition = transform.position;
            // D�truire l'agent
            Destroy(gameObject);
            Instantiate(_CubeToLife, deathPosition, Quaternion.identity);
        }

        if (!hasSpawned && collision.gameObject.CompareTag("Agent"))
        {   
            hasSpawned = true;
            Vector3 randomOffset = Random.insideUnitSphere * maxOffsetDistance;

            // Calculer la position de spawn avec l'offset al�atoire par rapport au point de collision
            Vector3 spawnPosition = collision.contacts[0].point + randomOffset;

            // Instancier le nouvel agent � la position calcul�e
            Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
