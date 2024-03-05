using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class FeedBack : MonoBehaviour
{
    [Header("Reference")]
    public AgentToTrace _AgentTraceScript;
    public Text _AgentUsed;
   


    [Header("Value")]
    public float _OffSetX;
    public float _OffSetY;


    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        _AgentUsed.transform.position = new Vector3(mouseScreenPosition.x + _OffSetX, mouseScreenPosition.y + _OffSetY, 0);


        if (Input.GetMouseButton(0))
        {
            _AgentUsed.color = new Color(_AgentUsed.color.r, _AgentUsed.color.g, _AgentUsed.color.b, 0.8f);
        }
        else
        {
            _AgentUsed.color = new Color(_AgentUsed.color.r, _AgentUsed.color.g, _AgentUsed.color.b, 0f);
        }



        _AgentUsed.text = " Agent Used : " + _AgentTraceScript._numberAgentNeeded;
    }
}
