using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioMixerSnapshot muffledSnapshot;
    public AudioMixerSnapshot normalSnapshot;
    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        normalSnapshot.TransitionTo(0.01f);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        muffledSnapshot.TransitionTo(0.01f);
        Cursor.lockState = CursorLockMode.Confined;
    }
}
