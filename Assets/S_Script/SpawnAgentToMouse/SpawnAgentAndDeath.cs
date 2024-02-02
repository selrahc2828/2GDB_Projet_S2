using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAgentAndDeath : MonoBehaviour
{
    [Header("r�f�rence")]
    public GameObject agentPrefab;


    [Header("Param�tre")]
    public float _NumberOfCollision;
    public float _MaxTimeBeforeDeath;
    public float _TimeBeforeDeath;
    private float _Time;
    public float maxOffsetDistance = 1f;

    private bool hasSpawned = false;

    //private int collisionCount = 0;

    private void Start()
    {
        _TimeBeforeDeath = _MaxTimeBeforeDeath;
    }


    private void Update()
    {
        // Add second for the timer
        _Time += Time.deltaTime;

        if (_Time >= _TimeBeforeDeath && hasSpawned == false)
        {
            hasSpawned = true;
            Vector3 randomOffset = Random.insideUnitSphere * maxOffsetDistance;

            // Calculer la position de spawn avec l'offset al�atoire par rapport au point de collision
            Vector3 spawnPosition = transform.position + randomOffset;

            // Instancier le nouvel agent � la position calcul�e
            Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

    }

}
