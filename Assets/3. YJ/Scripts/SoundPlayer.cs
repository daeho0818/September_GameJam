using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public bool isMuted = false;
    public void PlaySound(AudioClip sound)
    {
        if(!isMuted)
            SoundManager.Instance.PlaySound(sound);
    }
    public void PlayMusic(AudioClip music)
    {
        if (!isMuted)
            SoundManager.Instance.PlayMusic(music);
    }
}
