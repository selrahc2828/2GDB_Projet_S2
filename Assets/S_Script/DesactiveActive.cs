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

    public List<ObjectToggleInfo> objectsToToggle; // Liste des objets � d�sactiver/r�activer
   

    private void Start()
    {

        // D�marrez la coroutine pour basculer les objets � des intervalles al�atoires
        StartCoroutine(ToggleObjectsRandomly());

    }

    // Coroutine pour d�sactiver/r�activer les objets � des intervalles al�atoires
    private IEnumerator ToggleObjectsRandomly()
    {
        foreach (ObjectToggleInfo objInfo in objectsToToggle)
        {
            StartCoroutine(ToggleObject(objInfo));
        }

        yield return null;
    }

    // Coroutine pour g�rer le basculement d'un objet individuel
    private IEnumerator ToggleObject(ObjectToggleInfo objInfo)
    {
        while (true)
        {
            // Attendez un intervalle al�atoire pour cet objet
            yield return new WaitForSeconds(Random.Range(objInfo.minInterval, objInfo.maxInterval));

            // Basculez l'�tat actif de l'objet
            objInfo.obj.SetActive(!objInfo.obj.activeSelf);
        }
    }
}
