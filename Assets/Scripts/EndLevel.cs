using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public bool isSun = false;
    public AudioClip chomp;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("NextSceneIndex ="+nextSceneIndex);
        // Debug.Log("SceneCount ="+SceneCount);
        if (collision.tag == "Player")                                                       // check if player
        {
            audioSource.PlayOneShot(chomp);
            GetComponent<SpriteRenderer>().enabled = false;
            if(isSun)
            {
                GameObject.Find("sunbeam").GetComponent<Animator>().SetTrigger("Consume");
            }
            StartCoroutine(loadScene());
        }
    }

    private IEnumerator loadScene()
    {
        yield return new WaitForSeconds(1);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int SceneCount = SceneManager.sceneCountInBuildSettings;

        if (SceneCount > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);                                     // go to a next level
        }
        else
        {
            SceneManager.LoadScene("Credits");                                         // go to credits
        }

    }
}
