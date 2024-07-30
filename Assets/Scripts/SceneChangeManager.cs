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
        //�V���O���g��
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
    /// �V�[����؂�ւ���@�\
    /// </summary>
    /// <param name="sceneIndex">BuildSettings�ɐݒ肵������</param>
    public void SceneChangeAsync(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        Debug.Log($"loadScene[{sceneIndex}]");
    }
}
