using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    // Scene Names
    const string BOOTSTRAP = "Bootstrap";
    const string LOGIN = "Login";
    const string MAIN_MENU = "Main Menu";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap() { if (SceneManager.GetActiveScene().name != BOOTSTRAP) SceneManager.LoadScene(BOOTSTRAP); }

    public void ExitApp() => Application.Quit();

    public void Logout()
    {
        // do the logout
        // PlayerPrefs.DeleteKey("JWT");
        SceneManager.LoadScene(LOGIN);
    }

    // for buttons
    public void GoToMainMenu() => GoTo(SceneTitle.MainMenu);

    public void GoToScene(SceneTitle scene) => scene == SceneTitle.Login ? Logout() : SceneManager.LoadScene(GetSceneName(scene));

    public string GetSceneName(SceneTitle title) => title switch
    {
        SceneTitle.Bootstrap => BOOTSTRAP,
        SceneTitle.Login => LOGIN,
        SceneTitle.MainMenu => MAIN_MENU,
        _ => MAIN_MENU
    };
}


public enum SceneTitle
{
    Bootstrap,
    Login,
    MainMenu
}