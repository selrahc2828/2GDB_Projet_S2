using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RecognizeItsSelf : MonoBehaviour
{
    public AgentToTrace _TraceScript;
    public NavMeshAgent _selfAgent;


    private void Awake()
    {
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
    }

    private void Update()
    {
        //IsInList();
    }


    public void IsInList()
    {
        
        List<List<Vector3>> allAgentLists = _TraceScript._listeOfListePositionTrace;

      
        for (int i = 0; i < allAgentLists.Count; i++)
        {
            List<Vector3> agentList = allAgentLists[i];

            Debug.Log("DictionaryFound");
            foreach (Vector3 position in agentList)
            {
                Debug.Log("RecupListDic");

                if (Vector3.Distance(_selfAgent.transform.position, position) < 0.1f)
                {
                    Debug.Log("ListeIndex " + i);
                    return;
                }
            }
        }
        Debug.Log("L'agent n'est pas présent dans les listes.");
    }
}
