using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    static SceneChangeManager _instance;
    public static SceneChangeManager Instance => _instance;
    [SerializeField] Ease _easeSceneChange;

    void Awake()
    {
        //シングルトン
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
    public void SceneChangeAsync(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        Debug.Log($"loadScene[{sceneIndex}]");
    }
}
