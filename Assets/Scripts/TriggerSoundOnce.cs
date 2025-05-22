using UnityEngine;

public class TriggerSoundOnce : MonoBehaviour
{
    public AudioSource targetAudioSource;
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            if (targetAudioSource != null)
            {
                targetAudioSource.Play();
                hasPlayed = true;
            }
        }
    }
}