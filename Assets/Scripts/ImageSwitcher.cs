using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    [SerializeField, Tooltip("変化させるイメージ")] Image _switcherImage;
    [SerializeField] Sprite _aciveImage;
    [SerializeField] Sprite _negativeImage;
    [SerializeField] SwitchImageType _switchImageType;
    [SerializeField] bool _simpleInitialImageSet = true;
    bool _simpleImageActive = true;
    Animator _animator;
    private void Start()
    {
        if (!_switcherImage)
        {
            Debug.LogWarning($"not aquipped switcher image in [{gameObject.name}]");
            return;
        }
        if (!_negativeImage)
        {
            Debug.LogWarning($"not aquipped active image in [{gameObject.name}]");
            return;
        }
        if (!_aciveImage)
        {
            Debug.LogWarning($"not aquipped negative image in [{gameObject.name}]");
            return;
        }
        if (_switchImageType == SwitchImageType.Simple)
        {
            _simpleImageActive = _simpleInitialImageSet;
            if (_simpleInitialImageSet)
            {
                _switcherImage.sprite = _aciveImage;
            }
            else
            {
                _switcherImage.sprite = _negativeImage;
            }
        }
        else if (_switchImageType == SwitchImageType.Animator)
        {
            _animator = GetComponent<Animator>();
        }
        else if(_switchImageType == SwitchImageType.BGM)
        {
            if(SaveManager.Instance.GetSaveData._muteBGM)
            {
                _switcherImage.sprite = _negativeImage;
            }
            else
            {
                _switcherImage.sprite = _aciveImage;
            }
        }
        else
        {
            if (SaveManager.Instance.GetSaveData._muteSE)
            {
                _switcherImage.sprite = _negativeImage;
            }
            else
            {
                _switcherImage.sprite = _aciveImage;
            }
        }
    }
    /// <summary>
    /// Sprite変更をする機能
    /// </summary>
    public void SwitchImageActive()
    {
        switch (_switchImageType)
        {
            case SwitchImageType.Simple:
                _simpleImageActive = !_simpleImageActive;
                if (_simpleImageActive)
                {
                    _switcherImage.sprite = _aciveImage;
                }
                else
                {
                    _switcherImage.sprite = _negativeImage;
                }
                break;
            case SwitchImageType.Animator:
                if(_animator && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    if (_animator.GetBool("IsActive"))
                    {
                        _switcherImage.sprite = _aciveImage;
                    }
                    else
                    {
                        _switcherImage.sprite = _negativeImage;
                    }
                }
                break;
            case SwitchImageType.SE:
                if (SEAudioManager.Instance.GetIsMuteSE)
                {
                    _switcherImage.sprite = _negativeImage;
                }
                else
                {
                    _switcherImage.sprite = _aciveImage;
                }
                break;
            case SwitchImageType.BGM:
                if (BGMAudioManager.Instance.GetIsMuteBGM)
                {
                    _switcherImage.sprite = _negativeImage;
                }
                else
                {
                    _switcherImage.sprite = _aciveImage;
                }
                break;
        }
    }
}
