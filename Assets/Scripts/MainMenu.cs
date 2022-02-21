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
public class MainMenu : MonoBehaviour
{
    
    // When TestLevel is pressed, we take the user to the testing level
    public void Testing () {
        SceneManager.LoadScene("scene_LevelTesting");
    }

        // When TestLevel is pressed, we take the user to the game world
    public void LevelOne () {
        SceneManager.LoadScene("scene_Game");
    }

    public void LevelOneDashTwo()
    {
        SceneManager.LoadScene("scene_Game-1-2");
    }

    // When quit is pressed anywhere in the game, the app closes
    public void Quit ()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit ();
    #endif
    }
}
