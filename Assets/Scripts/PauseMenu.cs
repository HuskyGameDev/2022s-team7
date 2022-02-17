using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles Menu Functionality
// Author: Ian Hughes
// Tutorials Followed: 
// 1) https://www.youtube.com/watch?v=zc8ac_qUXQY
// 2) https://www.youtube.com/watch?v=YOaYQrN1oYQ
// 3) https://www.youtube.com/watch?v=JivuXdrIHK0
// 4) https://docs.unity3d.com/ScriptReference/SceneManagement.Scene-name.html
public class PauseMenu : MonoBehaviour {

    public GameObject PauseMenuUI;
    public static bool GameIsPaused = false;

   void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
   }

   public void Resume (){
       PauseMenuUI.SetActive(false);
       Time.timeScale = 1f;
       GameIsPaused = false;
    }

   void Pause () {
       PauseMenuUI.SetActive(true);
       Time.timeScale = 0f;
       GameIsPaused = true;
    }

    // In the in game pause menu, the user is taken to the home screen with this button
    public void Home () {
        // public static bool GameIsPaused = false;
        SceneManager.LoadScene("scene_MainMenu");
    }

    // When quit is pressed anywhere in the game, the app closes
    public void Quit () {
        Application.Quit ();
    }
}
