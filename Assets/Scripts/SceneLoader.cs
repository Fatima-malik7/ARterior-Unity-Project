using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName); // This helps debug
        SceneManager.LoadScene(sceneName);
    }
}
