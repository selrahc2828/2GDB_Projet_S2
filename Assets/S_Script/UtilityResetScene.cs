using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityResetScene : MonoBehaviour
{
    void Update()
    {
        // Vérifier si la touche "R" est enfoncée
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Relancer la scène actuelle
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
