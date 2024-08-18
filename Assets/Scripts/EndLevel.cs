using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int SceneCount = SceneManager.sceneCountInBuildSettings;
        // Debug.Log("NextSceneIndex ="+nextSceneIndex);
        // Debug.Log("SceneCount ="+SceneCount);
        if (collision.tag == "Player")                                                       // check if player
        {
            if (SceneCount > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);                                     // go to a next level
            }
            else
            {
                SceneManager.LoadScene("MainMenu");                                         // go to main menu
            }
        }
    }
}
