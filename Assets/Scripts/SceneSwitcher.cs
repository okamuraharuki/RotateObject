using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    static SceneSwitcher _instance;
    public static SceneSwitcher Instance => _instance;
    [SerializeField] Ease _easeSceneChange;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// シーンを切り替える機能
    /// </summary>
    /// <param name="sceneIndex">BuildSettingsに設定した順番</param>
    public async void SceneChangeAsync(int sceneIndex)
    {
        await FadeManager.Instance.FadeAsync(null);
        SceneManager.LoadSceneAsync(sceneIndex);
        Debug.Log($"loadScene[{sceneIndex}]");
        await FadeManager.Instance.UnFadeAsync(null);
        Debug.Log("unfade");
    }
}
