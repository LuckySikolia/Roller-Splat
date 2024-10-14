using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // called when button is clicked
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); //load the first level
        Debug.Log("Level One Loaded");
    }
}
