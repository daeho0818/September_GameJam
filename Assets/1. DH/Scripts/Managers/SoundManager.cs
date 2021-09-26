using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;
    public AudioClip landSound;
    public AudioClip winSound;
    public AudioClip pauseSound;
    public AudioClip rewindSound;
    public AudioClip objectSpawnSound;
    public AudioClip mainTheme;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }
    public void PlayMusic(AudioClip music, float volume = 1f)
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
        if (sound == landSound)
            PlaySound(sound, 0.4f);
        else if (sound == rewindSound)
            PlaySound(sound, 0.6f);
        else
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
