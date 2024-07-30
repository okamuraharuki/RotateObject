using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SEAudioManager : MonoBehaviour
{
    static SEAudioManager _instance;
    public static SEAudioManager Instance => _instance;
    [SerializeField, Tooltip("SE���܂Ƃ߂�z��")] List<AudioClip> _seClips;
    [SerializeField, Tooltip("SEVolume�𒲐߂���X���C�_�[")] Slider _seSlider;
    AudioSource _audioSource;
    float _volumeSE = 0.5f;
    /// <summary>
    /// �Z�[�u�p
    /// </summary>
    public float GetVolumeSE => _volumeSE;
    bool _isMuteSE = false;
    public bool GetIsMuteSE => _isMuteSE;
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
    void Start()
    {
        //SaveManager����f�[�^��荞��
        _volumeSE = SaveManager.Instance.GetSaveData._volumeSE;
        _isMuteSE = SaveManager.Instance.GetSaveData._muteSE;

        //Audiomanager������
        if (_seSlider)
        {
            _seSlider.value = (int)Mathf.Abs(_volumeSE * 10);
        }
        _audioSource = GetComponent<AudioSource>();
        Debug.Log("Initialize Audio");
    }
    /// <summary>
    /// SE��炷
    /// </summary>
    /// <param name="indexClipSE">seClips���ł̊��蓖�Ĕԍ�</param>
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
    /// SE�̉��ʂ�ύX����/�{�^���ŌĂ�
    /// </summary>
    /// <param name="volume">PlayOneShot��volumeScale��0-1�X�P�[���̂���0.1�{</param>
    public void ChangeSEVolume(float volume)
    {
        if (!_audioSource)
        {
            return;
        }

        _volumeSE = volume * 0.1f;
    }
    /// <summary>
    /// SE�̉��ʂ��~���[�g�ɂ���
    /// </summary>
    public void SwitchMuteSE()
    {
        _isMuteSE = !_isMuteSE;
    }
}
