using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(AudioClip sound)
    {
        SoundManager.Instance.PlaySound(sound);
    }
}
