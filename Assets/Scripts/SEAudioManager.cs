using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SEAudioManager : MonoBehaviour
{
    static SEAudioManager _instance;
    public static SEAudioManager Instance => _instance;
    [SerializeField, Tooltip("SEをまとめる配列")] List<AudioClip> _seClips;
    [SerializeField, Tooltip("SEVolumeを調節するスライダー")] Slider _seSlider;
    AudioSource _audioSource;
    float _volumeSE = 0.5f;
    /// <summary>
    /// セーブ用
    /// </summary>
    public float GetVolumeSE => _volumeSE;
    bool _isMuteSE = false;
    public bool GetIsMuteSE => _isMuteSE;
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
    void Start()
    {
        //SaveManagerからデータ取り込み
        _volumeSE = SaveDataManager.Instance.GetSaveData._volumeSE;
        _isMuteSE = SaveDataManager.Instance.GetSaveData._muteSE;

        //Audiomanager初期化
        if (_seSlider)
        {
            _seSlider.value = (int)Mathf.Abs(_volumeSE * 10);
        }
        _audioSource = GetComponent<AudioSource>();
        Debug.Log("Initialize Audio");
    }
    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="indexClipSE">seClips内での割り当て番号</param>
    public void PlayClipSE(int indexClipSE)
    {
        if (indexClipSE < _seClips.Count)
        {
            try
            {
                _audioSource.PlayOneShot(_seClips[indexClipSE], _isMuteSE ? 0 : _volumeSE);
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
        if (!_audioSource)
        {
            return;
        }

        _volumeSE = volume * 0.1f;
    }
    /// <summary>
    /// SEの音量をミュートにする
    /// </summary>
    public void SwitchMuteSE()
    {
        _isMuteSE = !_isMuteSE;
    }
}
