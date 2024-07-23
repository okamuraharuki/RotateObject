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
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        ImageInitialize();
    }
    void ImageInitialize()
    {
        if (!_fadeImage)
        {
            try
            {
                _fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
            }
            catch
            {
                Debug.Log("not aquipped fadeImage\ninstantiate fadeImage");
                GameObject fadeObj = new GameObject();
                fadeObj.name = "FadeImage";
                fadeObj.tag = "FadeImage";
                _fadeImage = fadeObj.AddComponent<Image>();
                _fadeImage.rectTransform.sizeDelta = new Vector3(Screen.width, Screen.height);
                _fadeImage.color = Color.black;
                _fadeImage.gameObject.SetActive(false);
            }
        }
    }
    public async Task FadeAsync(Ease? ease)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color *= new Color(1, 1, 1, 0);
        await _fadeImage.DOFade(1, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
    }
    public async Task UnFadeAsync(Ease? ease)
    {
        if(!_fadeImage)
        {//                           Ç±ÇÍÇæÇ∆missingéQè∆missingÇ≈ÇÈ
            Debug.Log("fadeImage initialize");
            ImageInitialize();
        }
        _fadeImage.color *= new Color(1, 1, 1, 1);
        await _fadeImage.DOFade(0, _fadeTime).SetEase(ease != null ? (Ease)ease : _defaultDotweenEase).AsyncWaitForCompletion();
        _fadeImage.gameObject.SetActive(false);
    }
}
