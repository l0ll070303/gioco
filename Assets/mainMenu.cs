using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadSceneAsync("z");
    }
    public void PlayTutorial(){
        SceneManager.LoadSceneAsync("Tutorial");
    }
    public void PlaymainMenu(){
        SceneManager.LoadSceneAsync("mainMenu");
    }
    public void PlayEnd(){
        SceneManager.LoadSceneAsync("end");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
