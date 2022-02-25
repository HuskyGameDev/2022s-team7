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
// 5) https://www.youtube.com/watch?v=eC05j7rh_LM
public class PauseMenu : MonoBehaviour {

    public static bool isGamePaused = false;

    [SerializeField] GameObject pauseMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (isGamePaused){
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    // In the in game pause menu, the user is taken to the home screen with this button
    public void Home () {
        ResumeGame();
        SceneManager.LoadScene("scene_MainMenu");
    }

    // When quit is pressed anywhere in the game, the app closes
    public void Quit ()
    {
    ResumeGame();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit ();
    #endif
    }
}
