using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance;
    public static AudioManager Instance => _instance;
    [SerializeField, Tooltip("シーンが読み込まれてからBGMが流れるまでの秒数")] float _startTimeBGM = 1.0f;
    [SerializeField, Tooltip("BuildSettingsにシーンが登録されている番号は\nそのシーンの初期BGMになるので注意")] AudioClip[] _bgmClips;
    [SerializeField] AudioClip[] _seClips;
    AudioSource _audioSource;
    float _volumeSE = 0.5f;
    public float VolumeSE => _volumeSE;

    float _volumeBGM = 0.5f;
    public float VolumeBGM => _volumeBGM;
    bool _isMuteSE = false;
    public bool IsMuteSE => _isMuteSE;
    bool _isMuteBGM = false;
    public bool IsMuteBGM => _isMuteBGM;
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
    async void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        await InitializeBGM();
        Debug.Log("Initialize Audio");
    }
    async Task InitializeBGM()
    {
        _audioSource.Stop();
        _audioSource.volume = _volumeBGM;
        await Task.Delay((int)_startTimeBGM * 1000);
        _audioSource.Play();
    }
    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="indexClipSE"></param>
    public void PlayClipSE(int indexClipSE)
    {
        if(indexClipSE < _seClips.Length)
        {
            try
            {
                _audioSource.PlayOneShot(_seClips[indexClipSE],_isMuteSE ? 0 : _volumeSE);
            }
            catch
            {
                Debug.LogWarning($"_seClips[{indexClipSE}] is null");
            }
        }
        else
        {
            Debug.Log($"_seClips[{indexClipSE}] not aquipped");
        }
    }
    /// <summary>
    /// SEの音量を変更する/ボタンで呼ぶ
    /// </summary>
    /// <param name="volume">PlayOneShotのvolumeScaleが0-1スケールのため0.1倍</param>
    public void ChangeSEVolume(float volume)
    {
        _volumeSE = volume * 0.1f;
    }
    public void SwitchMuteSE()
    {
        _isMuteSE = !_isMuteSE;
    }
    /// <summary>
    /// BGMの音量を変更する/ボタンで呼ぶ
    /// </summary>
    /// <param name="volume">AudioSorceのvolumeScaleが0-1スケールのため0.1倍</param>
    public void ChangeBGMVolume(float volume)
    {
        if(_isMuteBGM)
        {
            _volumeBGM = volume;
        }
        else
        {
            _audioSource.volume = volume * 0.1f;
            _volumeBGM = volume * 0.1f;
        }
    }
    public void SwitchMuteBGM()
    {
        _isMuteBGM = !_isMuteBGM;
        _audioSource.volume = 0;
    }
    /// <summary>
    /// BGMを変更する
    /// </summary>
    /// <param name="indexClipBGM"></param>
    public void ChangeClipBGM(int indexClipBGM)
    {
        if(indexClipBGM < _bgmClips.Length)
        {
            _audioSource.clip = _seClips[indexClipBGM];
            _audioSource.Play();
        }
        else
        {
            Debug.Log($"_bgmClips[{indexClipBGM}] not aquipped");
        }
    }
}
