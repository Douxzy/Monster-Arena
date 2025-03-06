using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer masterMixer; // Assigner l'AudioMixer dans Unity

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetMusicVolume(float volume)
    {
        if (masterMixer != null)
        {
            bool exists = masterMixer.GetFloat("MusicVolume", out float currentValue);
            if (exists)
            {
                masterMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 20, volume));
            }
            else
            {
                Debug.LogError("❌ MusicVolume n'existe pas dans l'AudioMixer !");
            }
        }
        else
        {
            Debug.LogError("❌ AudioMixer non assigné dans l'Inspector !");
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (masterMixer != null)
        {
            bool exists = masterMixer.GetFloat("SFXVolume", out float currentValue);
            if (exists)
            {
                masterMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 20, volume));
            }
            else
            {
                Debug.LogError("❌ SFXVolume n'existe pas dans l'AudioMixer !");
            }
        }
        else
        {
            Debug.LogError("❌ AudioMixer non assigné dans l'Inspector !");
        }
    }
}
