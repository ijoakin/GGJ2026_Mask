using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}