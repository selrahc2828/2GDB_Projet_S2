using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityResetScene : MonoBehaviour
{
    void Update()
    {
        // V�rifier si la touche "R" est enfonc�e
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Relancer la sc�ne actuelle
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
