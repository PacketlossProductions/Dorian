using UnityEngine;

public class AudioSourceDistance : MonoBehaviour
{
    public float minDist = 3;
    public float maxDist = 20;

    private AudioSource audioSource;
    public float dist = 0.0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position, Camera.main.transform.position) - Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        if (dist < minDist)
        {
            audioSource.volume = 1;
        }
        else if (dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1 - ((dist - minDist) / (maxDist - minDist));
        }
    }
}
