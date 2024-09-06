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
    /// フェードをする機能
    /// </summary>
    /// <param name="ease">フェードする変化量　引数なしの場合設定されたデフォルトが呼ばれる</param>
    public async Task FadeAsync(Ease? ease)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color *= new Color(1, 1, 1, 0);
        await _fadeImage.DOFade(1, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
    }
    /// <summary>
    /// アンフェードをする機能
    /// </summary>
    /// <param name="ease">アンフェードする変化量　引数なしの場合設定されたデフォルトが呼ばれる</param>
    public async Task UnFadeAsync(Ease? ease)
    {
        _fadeImage.color *= new Color(1, 1, 1, 1);
        await _fadeImage.DOFade(0, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
        _fadeImage.gameObject.SetActive(false);
    }
}
