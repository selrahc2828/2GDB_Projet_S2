using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JaugeScript : MonoBehaviour
{
    // La liste des agents cr��s
     private List<GameObject> agents = new List<GameObject>();

    public Slider _SliderJauge;
    public GameObject _Panel;
    public float _NumberMax;
    

    private void Update()
    {
        // Mettre � jour la liste des agents
        UpdateAgentList();
        //Mettre pour le slider 
        UpdateJaugeValue();

        if (_SliderJauge.value <= 0f)
        {
            _Panel.SetActive(true);
        }

        _NumberMax = _SliderJauge.maxValue;

    }

    private void UpdateAgentList()
    {
        // R�cup�rer tous les agents dans la sc�ne
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Agent");

        // Parcourir tous les agents dans la liste
        for (int i = agents.Count - 1; i >= 0; i--)
        {
            GameObject agent = agents[i];

            // V�rifier si l'agent existe toujours dans la sc�ne
            if (!System.Array.Exists(allAgents, obj => obj == agent))
            {
                // L'agent n'existe plus, donc le retirer de la liste
                agents.RemoveAt(i);
            }
        }

        // Ajouter les nouveaux agents � la liste
        foreach (GameObject agent in allAgents)
        {
            if (!agents.Contains(agent))
            {
                agents.Add(agent);
            }
        }
    }

    public void UpdateJaugeValue()
    {
        // Mettre � jour la valeur de la jauge en fonction du nombre d'agents dans la liste
        _SliderJauge.value = _NumberMax - agents.Count;
    }
}
