using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHelper : MonoBehaviour
{
    public GameObject[] windows;

    public void ShowWindow(int windowIndex)
    {
        windows[windowIndex].SetActive(true);
    }
    public void HideWindow(int windowIndex)
    {
        windows[windowIndex].SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
