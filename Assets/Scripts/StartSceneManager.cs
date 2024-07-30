using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    [SerializeField] GameObject _initialAnswerObject;
    [SerializeField] GameObject _initialRotationObject;
    private void Start()
    {
        //�^�C�g���V�[���ɍ��킹���I�u�W�F�N�g�̏�����
        RotationManager.Instance.Initialize(_initialAnswerObject.transform.rotation, _initialRotationObject);
    }
    public async void ChangeNormal()
    {
        BGMAudioManager.Instance.StopBGM();
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
