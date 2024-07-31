using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BGMAudioManager : MonoBehaviour
{
    static BGMAudioManager _instance;
    public static BGMAudioManager Instance => _instance;
    [SerializeField, Tooltip("�V�[�����ǂݍ��܂�Ă���BGM�������܂ł̕b��")] float _startTimeBGM = 1.0f;
    [SerializeField, Tooltip("BGM���܂Ƃ߂�z��\nBuildSettings�ɃV�[�����o�^����Ă���ԍ���\n���̃V�[���̏���BGM�ɂȂ�̂Œ���")] List<AudioClip> _bgmClips;
    [SerializeField, Tooltip("BGMVolume�𒲐߂���X���C�_�[")] Slider _bgmSlider;
    AudioSource _audioSource;
    float _volumeBGM = 0.2f;
    /// <summary>
    /// �Z�[�u�p
    /// </summary>
    public float GetVolumeBGM => _volumeBGM;
    bool _isMuteBGM = false;
    public bool GetIsMuteBGM => _isMuteBGM;
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
    async void Start()
    {
        //SaveManager����f�[�^��荞��
        _volumeBGM = SaveDataManager.Instance.GetSaveData._volumeBGM;
        _isMuteBGM = SaveDataManager.Instance.GetSaveData._muteBGM;

        //Audiomanager������
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
    /// BGM�̉��ʂ�ύX����/�{�^���ŌĂ�
    /// </summary>
    /// <param name="volume">AudioSorce��volumeScale��0-1�X�P�[���̂���0.1�{</param>
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
    /// BGM�̉��ʂ��~���[�g�ɂ���
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
    /// BGM��ύX���ė���
    /// </summary>
    /// <param name="indexClipBGM">bgmClips���ł̊��蓖�Ĕԍ�</param>
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
