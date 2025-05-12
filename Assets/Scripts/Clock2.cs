using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock2 : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clock;
    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayClick2()
    {
        audio.PlayOneShot(clock);
    }
}
