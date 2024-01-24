using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public jauge jaugeScript; //script jauge, on s'en sert pour mesurer la jauge et attribuer un boost au joueur en fonction
    private float actualJauge; // variable dans laquelle on va mettre le niveau de la jeuge

    [SerializeField]
    private float maximumSpeed; //variable de vitesse maximum de base (hors boost)

    [SerializeField]
    private float rotationSpeed; //vitesse de rotation du personnage

    [SerializeField]
    private float jumpSpeed; //force du saut

    [SerializeField]
    private float jumpButtonGracePeriod; //couldown du saut

    [SerializeField]
    private Transform cameraTransform; //transform de la camera, on s'en sert pour calculer la vrai direction du joueur

    private CharacterController characterController; // charactercontroller, on s'en sert pour bouger :/
    private float ySpeed; //Variable qui va servir a tout ce qui touche au mouvement vertical (saut, gravité etc)
    private float originalStepOffset; //je suis pas sur mais en gros c'est un offset, on s'en sert dans le saut (je crois que c'est pour passer des escaliers ou un truc du genre)
    private float? lastGroundedTime; //variable qui va contenir le temps au dernier moment ou le joueur touchais le sol
    private float? jumpButtonPressedTime; //variable qui va contenir le temps au dernier moment ou le joueur touchais la touche saut

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //Input du joueur (zqsd) changeable dans le controller de project setting
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Determiner la direction des inputs
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        //Création de l'input magnitude, une valeur de 0 à 1 qui détermine le pourcentage de la vitesse max de base
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        //si le joueur press shift, on rallentis le mouvement par 2
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }
        
        //si la fonction critical boost renvoie true, booster la vitesse du player
        if (CriticalState_Boost())
        {
            inputMagnitude *= 2;
        }

        //déterminer la speed du joueur en fonction de la magnitude qu'on viens de calculer multiplié par la max speed
        float speed = inputMagnitude * maximumSpeed;

        //changer l'angle du mouvement en fonction de l'angle de la camera (concretement ça fais que si tu press gauche, tu ira toujours a gauche de ta camera)
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        //noramalise le vecteur direction
        movementDirection.Normalize();

        //création de la gravité
        ySpeed += Physics.gravity.y * Time.deltaTime;

        //création d'une variable qui sert a savoir le dermier moment ou le joueur touchais le sol
        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        //création d'une variable qui sert a savoir le dernier moment ou le joueur a press jump
        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        //si le joueur etait au sol récement
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod) 
        {
            //on s'assure que le stepOffset soit normal
            characterController.stepOffset = originalStepOffset;

            //on réduit la gravité
            ySpeed = -0.5f;

            //si le temps écoulé depuis le dernier press jump est inférieur au couldown (jumpButtonGracePeriod)
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                //on fais sauter le joueur en changeant sa YSpeed
                ySpeed = jumpSpeed;

                //rendre null les variable du temps depuis le derier saut, indiquant que celui ci c'est déroulé avec succes
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        //si le joueur etait pas au sol récement
        else
        {
            //On réduit le stepOffset à 0
            characterController.stepOffset = 0;
        }

        //on crée la velocity à partir de la direction multiplié par la speed
        Vector3 velocity = movementDirection * speed;

        //on ajoute a la velocity.Y le mouvement vertical (saut/chute)
        velocity.y = ySpeed;

        //on fais bouger le personnage
        characterController.Move(velocity * Time.deltaTime);

        //si le mouvement existe (le mec est pas immobile)
        if (movementDirection != Vector3.zero)
        {
            //On calcule l'angle de la direction du mouvement 
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            //On fais rotate le player pour qu'il se tourne dans la direction ou il avance
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    //fonction qui retourne un booleen, en fonction de la jauge
    private bool CriticalState_Boost()
    {
        //on assigne la jauge du script jauge a une variable
        actualJauge = jaugeScript.actualJauge;

        //si la jauge est a moins de 30%
        if(actualJauge <= 30)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //fonction pour enlever le curseur pendant le playmode
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}