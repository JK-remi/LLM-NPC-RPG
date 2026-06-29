using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnEndApp()
    {
        Application.Quit();
    }
}
