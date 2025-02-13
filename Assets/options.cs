using UnityEngine;
using UnityEngine.SceneManagement;

public class options : MonoBehaviour
{
    public void PlayGame2(){
        SceneManager.LoadSceneAsync("mainMenu");
    }
}
