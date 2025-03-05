using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer masterMixer;


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat ("volume", Mathf.Lerp(-80, 20, soundLevel));
    }
}
