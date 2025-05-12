using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeButton : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string master = "Master";
    public string music = "Music";
    public string sfx = "SFX";
    public string vo = "VO";
    public float stepsize = 1.0f;
    public float minimvolume = -80f;
    public float maximvolume = 20f;

    private bool musicMuted = false;
    private float musicLastVolume = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Adjustvolume(master, stepsize);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Adjustvolume(master, -stepsize);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Adjustvolume(music, stepsize);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Adjustvolume(music, -stepsize);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Adjustvolume(sfx, stepsize);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Adjustvolume(sfx, -stepsize);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Adjustvolume(vo, stepsize);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Adjustvolume(vo, -stepsize);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusicMute();
        }
    }
    
    void Adjustvolume(string name, float newvolume)
    {
        bool todobien = audioMixer.GetFloat(name, out float volume);
        float newdb = Mathf.Clamp((todobien ? volume : 0f) + newvolume, minimvolume, maximvolume);
        audioMixer.SetFloat(name, newdb);
        Debug.Log($"{name} es ahora {newdb}");

        if (name == music && !musicMuted)
            musicLastVolume = newdb;
    }

    void ToggleMusicMute()
    {
        if (!musicMuted)
        {
            if (audioMixer.GetFloat(music, out float current))
                musicLastVolume = current;

            audioMixer.SetFloat(music, minimvolume);
            musicMuted = true;
            Debug.Log("Music muted");
        }
        else
        {
            audioMixer.SetFloat(music, musicLastVolume);
            musicMuted = false;
            Debug.Log($"Music unmuted: restored to {musicLastVolume} dB");
        }
    }
}
