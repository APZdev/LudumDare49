using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;

    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
