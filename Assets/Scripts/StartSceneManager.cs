using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public async void ChangeNormal()
    {
        await FadeManager.Instance.FadeAsync(null);
        SceneChangeManager.Instance.SceneChangeAsync(1);
        await FadeManager.Instance.UnFadeAsync(null);
        Debug.Log("unfade");
        BGMAudioManager.Instance.ChangeClipBGM(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
