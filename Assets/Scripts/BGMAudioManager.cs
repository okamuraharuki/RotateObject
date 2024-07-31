using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BGMAudioManager : MonoBehaviour
{
    static BGMAudioManager _instance;
    public static BGMAudioManager Instance => _instance;
    [SerializeField, Tooltip("シーンが読み込まれてからBGMが流れるまでの秒数")] float _startTimeBGM = 1.0f;
    [SerializeField, Tooltip("BGMをまとめる配列\nBuildSettingsにシーンが登録されている番号は\nそのシーンの初期BGMになるので注意")] List<AudioClip> _bgmClips;
    [SerializeField, Tooltip("BGMVolumeを調節するスライダー")] Slider _bgmSlider;
    AudioSource _audioSource;
    float _volumeBGM = 0.2f;
    /// <summary>
    /// セーブ用
    /// </summary>
    public float GetVolumeBGM => _volumeBGM;
    bool _isMuteBGM = false;
    public bool GetIsMuteBGM => _isMuteBGM;
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
        //SaveManagerからデータ取り込み
        _volumeBGM = SaveDataManager.Instance.GetSaveData._volumeBGM;
        _isMuteBGM = SaveDataManager.Instance.GetSaveData._muteBGM;

        //Audiomanager初期化
        if (_bgmSlider)
        {
            _bgmSlider.value = (int)Mathf.Abs(_volumeBGM * 10);
        }
        _audioSource = GetComponent<AudioSource>();
        await InitializeBGM();
        Debug.Log("Initialize Audio");
    }
    async Task InitializeBGM()
    {
        _audioSource.Stop();
        if(_isMuteBGM)
        {
            _audioSource.volume = 0;
        }
        else
        {
            _audioSource.volume = _volumeBGM;
        }
        await Task.Delay((int)_startTimeBGM * 1000);
        ChangeClipBGM(0);
    }
    /// <summary>
    /// BGMの音量を変更する/ボタンで呼ぶ
    /// </summary>
    /// <param name="volume">AudioSorceのvolumeScaleが0-1スケールのため0.1倍</param>
    public void ChangeBGMVolume(float volume)
    {
        if (!_audioSource)
        {
            return;
        }

        if (_isMuteBGM)
        {
            _volumeBGM = volume;
        }
        else
        {
            _audioSource.volume = volume * 0.1f;
            _volumeBGM = volume * 0.1f;
        }
    }
    /// <summary>
    /// BGMの音量をミュートにする
    /// </summary>
    public void SwitchMuteBGM()
    {
        _isMuteBGM = !_isMuteBGM;
        if (_isMuteBGM)
        {
            _audioSource.volume = 0;
        }
        else
        {
            _audioSource.volume = _volumeBGM;
        }
    }
    /// <summary>
    /// BGMを変更して流す
    /// </summary>
    /// <param name="indexClipBGM">bgmClips内での割り当て番号</param>
    public void ChangeClipBGM(int indexClipBGM)
    {
        if (indexClipBGM < _bgmClips.Count)
        {
            _audioSource.clip = _bgmClips[indexClipBGM];
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"_bgmClips[{indexClipBGM}] not aquipped");
        }
    }
    public void StopBGM()
    {
        _audioSource.Stop();
    }
}
