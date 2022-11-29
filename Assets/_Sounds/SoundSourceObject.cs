using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class SoundSourceObject : MonoBehaviour
{
    public static SoundSourceObject Instance;

    public AudioListener AudioListener { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        AudioListener = GetComponent<AudioListener>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        AudioSource.playOnAwake = true;
        AudioSource.loop = true;

        AudioSource.Play();
    }
}
