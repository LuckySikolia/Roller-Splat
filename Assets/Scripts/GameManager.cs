using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // only have one game manager instance
    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;

    //particle system refreence
    public ParticleSystem winEffect;

    //void Start()
    //{
    //    //Load the start menu when the game starts
    //    SceneManager.LoadScene("StartMenu");
    //    Debug.Log("Start Menu Load");
    //    //SetupNewLevel();
    //}

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(singleton != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }
    
    public void CheckComplete()
    {
        bool isFinished = true;

        for(int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            //trigger particle effect before loading next level
            PlayWinEffect();

            //Delay the next level load slightly so the effect can play
            StartCoroutine(NextLevelWithDelay());
            //call next level
            //NextLevel();
        }
    }

    //play the particle effect for the win
    private void PlayWinEffect()
    {
        if(winEffect != null)
        {
            winEffect.Play();
        }
    }

    //Coroutine to wait before loading the next level
    private IEnumerator NextLevelWithDelay()
    {
        yield return new WaitForSeconds(5f);

        NextLevel();
    }




    //private void NextLevel()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 1)
    //    {
    //        SceneManager.LoadScene(1);
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    }

    //}

    private void NextLevel()
    {
        //get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //Load the next level based on the current index
        if (currentSceneIndex >= SceneManager.sceneCountInBuildSettings - 1) //check if this is the last level
        {
            //Go back to start screen
            SceneManager.LoadScene(0);
            Debug.Log("Game complete");
        }
        else
        {
            //load the next scene
            SceneManager.LoadScene(currentSceneIndex + 1);
            Debug.Log("Load New Level");
        }
    }


}
