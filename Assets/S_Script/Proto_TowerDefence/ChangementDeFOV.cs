using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangementDeFOV : MonoBehaviour
{

    public Camera _Camera; 
    public float _slowMo;
    public GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void Start()
    {
        _slowMo = _gameManager._slowMo;
        _Camera.fieldOfView = 60;
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Time.timeScale = _slowMo;

            _Camera.fieldOfView = 55;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1f;

            _Camera.fieldOfView = 60;
        }
    }
}
