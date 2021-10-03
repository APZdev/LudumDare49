using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip ambientNoise;
    public AudioClip[] squeakNoiseClips;

    private float squeakElapsedTime;

    private void Start()
    {
        audioSource.loop = true;
        audioSource.clip = ambientNoise;
        audioSource.Play();
    }

    private void Update()
    {
        squeakElapsedTime += Time.deltaTime;
        if (squeakElapsedTime > 10)
        {
            int rng = Random.Range(0, squeakNoiseClips.Length);
            audioSource.PlayOneShot(squeakNoiseClips[rng], 3.5f);
            squeakElapsedTime = 0;
        }
    }
}
