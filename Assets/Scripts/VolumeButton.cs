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
    private bool isMuffled = false;
    private float musicLastVolume = 0f;

    void Start()
    {
        
    }

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

        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleMuffled();
        }
    }

    void Adjustvolume(string name, float step)
    {
        if (audioMixer.GetFloat(name, out float currentDb))
        {
            float currentLinear = Mathf.Pow(10f, currentDb / 20f);

            float newLinear = currentLinear + (step * 0.01f);

            newLinear = Mathf.Clamp(newLinear, 0.0001f, 10f);

            if (Mathf.Approximately(newLinear, 0.0001f) && step < 0f)
            {
                Debug.Log($"{name} is already at minimum volume.");
                return;
            }

            float newDb = Mathf.Clamp(Mathf.Log10(newLinear) * 20f, minimvolume, maximvolume);
            audioMixer.SetFloat(name, newDb);

            Debug.Log($"{name} volume changed to {newDb} dB (linear: {newLinear})");

            if (name == music && !musicMuted)
                musicLastVolume = newDb;
        }
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

    void ToggleMuffled()
    {
        if (isMuffled)
        {
            audioMixer.FindSnapshot("Normal").TransitionTo(0.5f);
            isMuffled = false;
        } else
        {
            audioMixer.FindSnapshot("Muffled").TransitionTo(0.5f);
            isMuffled = true;
        }
    }
}
