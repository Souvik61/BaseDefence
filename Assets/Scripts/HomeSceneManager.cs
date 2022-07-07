using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(1);        
    }

}
