using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSceneChang : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeScene(0); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeScene(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeScene(5);
        }
    }

    void ChangeScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Index de scène invalide.");
        }
    }
}
