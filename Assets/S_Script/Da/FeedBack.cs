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
    public GameObject _deathBubblePrefab;

    [Header("Value")]
    public float _OffSetX;
    public float _OffSetY;

    [Header("BubbleParameter")]
    public float _scaleDuration = 0.5f;
    public float _destroyDelay = 0.2f;
    public float _maxScale = 10f;

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

    #region DeathBubble :D
    public void DeathBubble(Transform _position)
    {
        GameObject deathBubble = Instantiate(_deathBubblePrefab, _position.position, Quaternion.identity);

        StartCoroutine(ScaleAndDestroy(deathBubble.transform));
    }

    IEnumerator ScaleAndDestroy(Transform bubbleTransform)
    {
        float timer = 0.0f;
        Vector3 originalScale = bubbleTransform.localScale;

        // Scale up
        while (timer < _scaleDuration)
        {
            float scaleProgress = timer / _scaleDuration;
            float scale = Mathf.Lerp(1.0f, _maxScale, scaleProgress);
            bubbleTransform.localScale = originalScale * scale;

            yield return null;
            timer += Time.deltaTime;
        }

        // Ensure it reaches max scale
        bubbleTransform.localScale = originalScale * _maxScale;

        // Wait for destroy delay
        yield return new WaitForSeconds(_destroyDelay);

        // Scale down
        timer = 0.0f;
        while (timer < _destroyDelay)
        {
            float scaleProgress = timer / _destroyDelay;
            float scale = Mathf.Lerp(_maxScale, 0.0f, scaleProgress);
            bubbleTransform.localScale = originalScale * scale;

            yield return null;
            timer += Time.deltaTime;
        }

        bubbleTransform.localScale = Vector3.zero;
        Destroy(bubbleTransform.gameObject);
    }
    #endregion
}
