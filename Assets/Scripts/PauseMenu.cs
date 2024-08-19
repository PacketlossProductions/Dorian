using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public class PauseMenu : MonoBehaviour
{
    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject pm = FindMaybeDisabledGameObjectByName("UI","Pause Menu");
            pm.SetActive(true);
            pm.GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    public void ClosePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInput>().enabled = false;
            transform.gameObject.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerInput>().enabled = true;
        transform.parent.gameObject.GetComponent<PlayerInput>().enabled = false;
        transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Find a GO. It might be inactive.
    /// </summary>
    /// <param name="parentName">Name of parent GO. Must be active.</param>
    /// <param name="gameObjectName">Name of GO to find.</param>
    /// <returns>GO, or null.</returns>
    public static GameObject FindMaybeDisabledGameObjectByName(string parentName, string gameObjectName)
    {
        GameObject go = GameObject.Find(parentName);
        if (go == null)
        {
            return null;
        }
        Transform childTransform
            = go.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(
                t => t.name == gameObjectName
            );
        if (childTransform == null)
        {
            return null;
        }
        return childTransform.gameObject;
    }

}
