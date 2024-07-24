using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    static FadeManager _instance;
    public static FadeManager Instance => _instance;
    [SerializeField] Image _fadeImage;
    [SerializeField] float _fadeTime = 1f;
    [SerializeField] Ease _defaultDotweenEase = Ease.Linear;
    private void Awake()
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
    /// �t�F�[�h������@�\
    /// </summary>
    /// <param name="ease">�t�F�[�h����ω��ʁ@�����Ȃ��̏ꍇ�ݒ肳�ꂽ�f�t�H���g���Ă΂��</param>
    public async Task FadeAsync(Ease? ease)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color *= new Color(1, 1, 1, 0);
        await _fadeImage.DOFade(1, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
    }
    /// <summary>
    /// �A���t�F�[�h������@�\
    /// </summary>
    /// <param name="ease">�A���t�F�[�h����ω��ʁ@�����Ȃ��̏ꍇ�ݒ肳�ꂽ�f�t�H���g���Ă΂��</param>
    public async Task UnFadeAsync(Ease? ease)
    {
        _fadeImage.color *= new Color(1, 1, 1, 1);
        await _fadeImage.DOFade(0, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
        _fadeImage.gameObject.SetActive(false);
    }
}
