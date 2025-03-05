using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource src;
    public AudioClip menu;

    public static AudioManager instance;

    public void Menu()
    {
        src.clip = menu;
        src.Play();
    }
}
