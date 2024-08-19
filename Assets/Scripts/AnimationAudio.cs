using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationAudio : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(int clipIndex)
    {
        if(clipIndex < 0)
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
