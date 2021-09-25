using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;
    public AudioClip stage_clear;

    [SerializeField] GameObject[] source_objs;
    Queue<AudioSource> source_pool;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < source_objs.Length; i++)
        {
            source_pool.Enqueue(source_objs[i].GetComponent<AudioSource>());
        }
    }

    void Update()
    {

    }

    public void SoundPlay(AudioClip audio_clip, float volume = 0.5f, bool loop = false)
    {
        // audioSource.clip = audio_clip;
        // audioSource.volume = volume;
        // audioSource.loop = loop;
        // audioSource.Play();
    }
}
