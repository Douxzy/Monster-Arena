using UnityEngine;

public class MenuSFXManager : MonoBehaviour
{
    public static MenuSFXManager instance; // Singleton pour accès global
    public AudioSource audioSourceClick; // AudioSource pour le son du clic

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Click()
    {
        if (audioSourceClick != null)
        {
            audioSourceClick.Play();
        }
    }
}
