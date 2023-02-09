using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Invoke("PlayButton", 1);
            Cursor.visible = false;
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            Cursor.lockState = CursorLockMode.Locked;            
        }

    }

    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }
}
