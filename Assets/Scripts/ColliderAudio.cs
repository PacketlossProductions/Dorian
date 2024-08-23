using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ColliderAudio : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public int lastClip = -1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int clip = Random.Range(0, audioClips.Count);
        Debug.LogWarning("Picked clip " + clip + " from 0.." + (audioClips.Count - 1) + " for " + gameObject.name);
        PlaySound(clip);
        lastClip = clip;
    }

    void PlaySound(int clipIndex)
    {
        if (clipIndex < 0)
        {
            Debug.LogError("Invalid clip index " + clipIndex + " on " + gameObject.name);
            return;
        }
        if (clipIndex > audioClips.Count - 1)
        {
            Debug.LogError("Invalid clip index " + clipIndex + " on " + gameObject.name);
            return;
        }
        Debug.LogWarning("Playing " + audioClips[clipIndex].name + " for " + gameObject.name);
        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
}
