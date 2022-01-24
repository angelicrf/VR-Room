using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
  
    public void LoadSceneUsingName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
                
    }

    public void ReloadCurrentScene()
    {
        Debug.Log( "Scene is changed" );

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
