using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_MovementPlayer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public Transform m_Orientation;
    private Rigidbody rb;
    [SerializeField] public S_JaugeScript m_JaugeScript;


    [Header("Mouvement")]
    [SerializeField] public float m_WalkSpeed;
    [SerializeField] public float m_Forcepush;


    [Header("DebugHelper")]
    [SerializeField] public Text m_SpeedText;


    // Permet de récupéré les Inputs ET la direction du joueur
    float m_HorizontalInput;
    float m_VerticalInput;
    Vector3 m_MoveDirection;



    // Toute les Rotation du rigidbody son freez, a prendre en compte lors du code 
    void Start()
    {
        // Récupéré le rigidbody
        rb =  GetComponent<Rigidbody>();

       
    }

   
    void Update()
    {
        // Activer la fonction qui va faire bouger le player
        MovementPlayer();

        // Appeler la fonction Debug 
        DebugHelper();

        // Récupéré les Inputs de mouvement 
        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        m_VerticalInput = Input.GetAxisRaw("Vertical");

    }


    private void MovementPlayer()
    {
        // Ici je récupére l'orientation du joueur et je le multiplie par les Inputs du joueur
        m_MoveDirection = m_Orientation.forward * m_VerticalInput + m_Orientation.right * m_HorizontalInput;
        

        rb.AddForce(m_MoveDirection.normalized * m_WalkSpeed, ForceMode.Force);
        // J'ai appliquer cette force au rigidbody en ForceMode.Force en Fonction de la direction m_MoveDirection, si d'autre comportement son désiré changer la méthode "Force"

    }



    // Ici la parti du script DebugHelper n'est utiliser que pour la conception et l'aide a la fabrication du script
    public void DebugHelper()
    {
        // ------ Debug Speed Player ------
        m_SpeedText.text = "Speed : " + rb.velocity.magnitude.ToString("0.0") + " m/s";
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Cachette"))
       {
            m_JaugeScript.m_JaugeDecreaseRate = 5f;
            Debug.Log("Decrease rate to 5");
       }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cachette"))
        {
            m_JaugeScript.m_JaugeDecreaseRate = 0f;
            Debug.Log("Decrase rate to 0");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_JaugeScript.m_JaugeLevel <= 0)
        {
            rb.AddForce(m_Forcepush * transform.forward, ForceMode.Impulse);
        }
    }
}
