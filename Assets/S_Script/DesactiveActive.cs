using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveActive : MonoBehaviour
{
    [System.Serializable]
    public class ObjectToggleInfo
    {
        public GameObject obj;
        public float minInterval = 1f;
        public float maxInterval = 5f;
    }

    public List<ObjectToggleInfo> objectsToToggle; // Liste des objets à désactiver/réactiver
   

    private void Start()
    {

        // Démarrez la coroutine pour basculer les objets à des intervalles aléatoires
        StartCoroutine(ToggleObjectsRandomly());

    }

    // Coroutine pour désactiver/réactiver les objets à des intervalles aléatoires
    private IEnumerator ToggleObjectsRandomly()
    {
        foreach (ObjectToggleInfo objInfo in objectsToToggle)
        {
            StartCoroutine(ToggleObject(objInfo));
        }

        yield return null;
    }

    // Coroutine pour gérer le basculement d'un objet individuel
    private IEnumerator ToggleObject(ObjectToggleInfo objInfo)
    {
        while (true)
        {
            // Attendez un intervalle aléatoire pour cet objet
            yield return new WaitForSeconds(Random.Range(objInfo.minInterval, objInfo.maxInterval));

            // Basculez l'état actif de l'objet
            objInfo.obj.SetActive(!objInfo.obj.activeSelf);
        }
    }
}
