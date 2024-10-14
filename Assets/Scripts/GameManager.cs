using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // only have one game manager instance
    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;

    //particle system reference
    public ParticleSystem winEffect;
    private AudioSource winAudioSource; 


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

        //get the audioSource componenet
        winAudioSource = gameObject.GetComponent<AudioSource>();
     
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

            PlayWinEffect();///NB! Not working with level 2
            PlayWinSound(); 
            StartCoroutine(NextLevelWithDelay());

        }
    }

    //play the particle effect for the win
    private void PlayWinEffect()
    {
        Debug.Log($"PlayWinEffect called. winEffect is null: {winEffect == null}");
        if (winEffect != null)
        {
            winEffect.Play();
            Debug.Log("Particle effect played");
        }
        else
        {
            Debug.LogWarning("Win Effect Particle System is not assigned!");
        }
    }

    private void PlayWinSound()
    {
        if (winAudioSource != null)
        {
            winAudioSource.Play(); //play the sound effect
        }
        else
        {
            Debug.LogWarning("Win AudioSource is not assigned!");
        }
    }



    //Coroutine to wait before loading the next level
    private IEnumerator NextLevelWithDelay()
    {
        yield return new WaitForSeconds(3f);
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
