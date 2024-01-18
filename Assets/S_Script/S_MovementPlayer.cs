using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MovementPlayer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public Transform m_Orientation;
    private Rigidbody rb;


    [Header("Mouvement")]
    [SerializeField] public float m_WalkSpeed;


    // Permet de r�cup�r� les Inputs ET la direction du joueur
    float m_HorizontalInput;
    float m_VerticalInput;
    Vector3 m_MoveDirection;



    // Toute les Rotation du rigidbody son freez, a prendre en compte lors du code 
    void Start()
    {
        // R�cup�r� le rigidbody
        rb =  GetComponent<Rigidbody>();

       
    }

   
    void Update()
    {
        // Activer la fonction qui va faire bouger le player
        MovementPlayer();

        // R�cup�r� les Inputs de mouvement 
        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        m_VerticalInput = Input.GetAxisRaw("Vertical");
    }


    private void MovementPlayer()
    {
        // Ici je r�cup�re l'orientation du joueur et je le multiplie par les Inputs du joueur
        m_MoveDirection = m_Orientation.forward * m_VerticalInput + m_Orientation.right * m_HorizontalInput;
        

        rb.AddForce(m_MoveDirection.normalized * m_WalkSpeed, ForceMode.Force);
        // J'ai appliquer cette force au rigidbody en ForceMode.Force en Fonction de la direction m_MoveDirection, si d'autre comportement son d�sir� changer la m�thode "Force"

    }
}
