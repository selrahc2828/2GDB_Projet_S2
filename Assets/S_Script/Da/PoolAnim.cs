using UnityEngine;

public class PoolAnim : PoolScript
{

    [Header("Particule")]
    public GameObject _ParticulSys;


    [Header("Animation")]
    public Animator _animatorPool;
    public float _speed = 0.1f;
    private float _maxValue = 1.0f;
    private float _minValue = 0.0f;

    public bool _isInTrigger = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            _isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            _isInTrigger = false;
        }
    }

    private void Update()
    {
        if (_isInTrigger && !_infected)
        {
            _animatorPool.SetFloat("Blend", Mathf.Clamp(_animatorPool.GetFloat("Blend") + _speed * Time.deltaTime, _minValue, _maxValue));
            _ParticulSys.SetActive(true);
        }
        else
        {
            _animatorPool.SetFloat("Blend", Mathf.Clamp(_animatorPool.GetFloat("Blend") - _speed * Time.deltaTime, _minValue, _maxValue));
            _ParticulSys.SetActive(false);
        }
    }
}
