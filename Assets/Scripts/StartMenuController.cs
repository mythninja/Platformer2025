using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void OnHowToPlayClick()
    {
        SceneManager.LoadScene("HowToPlayScene");
    }
    public void OnSettingsClick()
    {
        SceneManager.LoadScene("SettingScene");
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene("StartupScene");
    }

}
