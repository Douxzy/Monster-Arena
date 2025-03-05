using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
        public void Start()
    {
            AudioManager.instance.Menu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Option()
    {
        SceneManager.LoadScene("OptionMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}