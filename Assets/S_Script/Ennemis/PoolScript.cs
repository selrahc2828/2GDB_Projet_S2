using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolScript : MonoBehaviour
{
    public bool _infected;

    [Header("ColorIfInfected")]
    [ColorUsage(false, true)]
    public Color _InitialColor;
    [ColorUsage(false, true)]
    public Color _InfectedColor;

    public MeshRenderer _meshRenderer;
    public MeshRenderer _meshRenderer2;
    
    private bool _previousInfectedState = false;


    public void CheckSurrounding()
    {
        _infected = false;
        Collider[] _enemiesInZone = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider enemy in _enemiesInZone)
        {
            if (enemy.CompareTag("AgentMechant") && enemy.GetComponent<BuzzKiller>() != null)
            {
                _infected = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckSurrounding();
    }

    private void Update()
    {
        LerpOnInfectedColor();
    }

    private void LerpOnInfectedColor()
    {
        if (_infected != _previousInfectedState)
        {
            StartCoroutine(StartColorTransition());
            _previousInfectedState = _infected;
        }
    }

    private IEnumerator StartColorTransition()
    {
        

        float transitionDuration = 1.0f;
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime = Time.time - startTime;

            float lerpFactor = _infected ? Mathf.Clamp01(elapsedTime / transitionDuration) : Mathf.Clamp01((transitionDuration - elapsedTime) / transitionDuration);
            Color lerpedColor = Color.Lerp(_InitialColor, _InfectedColor, lerpFactor);

            _meshRenderer.material.SetColor("_EmissionColor", lerpedColor);
            _meshRenderer.materials[1].SetColor("_Color", lerpedColor);

            _meshRenderer2.material.SetColor("_EmissionColor", lerpedColor);
            _meshRenderer2.materials[1].SetColor("_Color", lerpedColor);

            yield return null;
        }

       
    }

}
