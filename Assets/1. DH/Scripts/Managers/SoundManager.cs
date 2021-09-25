using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;
    public AudioClip stage_clear;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }
    public void PlayMusic(AudioClip music, float volume = 0.5f)
    {

        if (music != null)
        {
            if (musicSource.clip == music)
                return;
            musicSource.clip = music;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
    public void PlaySound(AudioClip sound)
    {
        PlaySound(sound, 1f);
    }
    public void PlaySound(AudioClip sound, float volume)
    {
        if (sound != null)
        {
            sfxSource.PlayOneShot(sound, volume);
        }
    }
}
