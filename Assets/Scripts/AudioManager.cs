using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance;
    public static AudioManager AudioSource => _instance;
    [SerializeField] float _startTimeBGM = 1.0f;
    [SerializeField] AudioClip[] _bgmClips;
    [SerializeField] AudioClip[] _seClips;
    AudioSource _audioSource;
    float _volumeSE = 3;
    float _volumeBGM = 0.1f;
    private void Awake()
    {
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
    public void PlayClip(int clipNum)
    {
        _audioSource.PlayOneShot(_seClips[clipNum], _volumeSE);
    }
    /// <param name="volume">PlayOneShotのvolumeScaleが0-1スケールのため0.1倍</param>
    public void ChangeSEVolume(float volume)
    {
        _volumeSE = volume * 0.1f;
    }
    /// <param name="volume">AudioSorceのvolumeScaleが0-1スケールのため0.1倍</param>
    public void ChangeBGMVolume(float volume)
    {
        _audioSource.volume = volume;
        _volumeBGM = volume * 0.1f;
    }
    public void ChangeAudioClip(int num)
    {
        _audioSource.clip = _seClips[num];
        _audioSource.Play();
    }
}
