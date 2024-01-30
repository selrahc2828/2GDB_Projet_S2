using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpawner : MonoBehaviour
{
    [Header("référence")]
    public GameObject agentPrefab;

    [Header("Paramètre")]
    public float _NumberOfCollision;
    public float _TimeBeforeDeath;
    private float _Time;
    public float maxOffsetDistance = 1f;

    private bool hasSpawned = false; 

    private int collisionCount = 0;


    private void Update()
    {
        // Add second for the timer
        _Time += Time.deltaTime;

        if (_Time >= _TimeBeforeDeath)
        {
            // destroy the GameObject if the timer is > than our value 
            Destroy(gameObject);
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
            // Détruire l'agent
            Destroy(gameObject);
        }

        if (!hasSpawned && collision.gameObject.CompareTag("Agent"))
        {   
            hasSpawned = true;
            Vector3 randomOffset = Random.insideUnitSphere * maxOffsetDistance;

            // Calculer la position de spawn avec l'offset aléatoire par rapport au point de collision
            Vector3 spawnPosition = collision.contacts[0].point + randomOffset;

            // Instancier le nouvel agent à la position calculée
            Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
