using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// this fn will load a different scene when it is called. the # refers to the index of the scene.
    /// create an idex on unity through File>Build settings>add open scenes (must have desired scene open to add)
    /// - when a start game button is pressed, OnLoadSceneCalled() must be called.
    /// Add an eveent to button to make it happen
    /// Note: you must also add the script to the button or another game object
    /// </summary>
    public void OnLoadSceneCalled()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLoadSceneCalled(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
