using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;
    public AudioClip stage_clear;

    [SerializeField] AudioSource[] audioSources;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
    }

    void Update()
    {

    }

    public void SoundPlay(AudioClip audio_clip, float volume = 0.5f, bool loop = false)
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = audio_clip;
                audioSource.volume = volume;
                audioSource.loop = loop;
                audioSource.Play();
            }
        }
    }
}
