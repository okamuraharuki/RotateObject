using UnityEngine;
using UnityEngine.UI;

public class MuteImageSwitcher : MonoBehaviour
{

    [SerializeField, Tooltip("変化させるミュートイメージ")] Image _muteSwitcherImage;
    [SerializeField] Sprite _speakerImage;
    [SerializeField] Sprite _muteImage;
    [SerializeField] AudioType _audioType;
    private void Start()
    {
        if (!_muteSwitcherImage)
        {
            Debug.LogWarning("not aquipped mute switcher image");
        }
        if (!_muteImage)
        {
            Debug.LogWarning("not aquipped mute image");
        }
        if (!_speakerImage)
        {
            Debug.LogWarning("not aquipped speaker image");
        }
    }
    /// <summary>
    /// ミュート切り替え時のSprite変更をする
    /// </summary>
    public void SwitchMuteSprite()
    {
        switch (_audioType)
        {
            case AudioType.SE:
                if (AudioManager.Instance.IsMuteSE)
                {
                    _muteSwitcherImage.sprite = _muteImage;
                }
                else
                {
                    _muteSwitcherImage.sprite = _speakerImage;
                }
                break;
            case AudioType.BGM:
                if (AudioManager.Instance.IsMuteBGM)
                {
                    _muteSwitcherImage.sprite = _muteImage;
                }
                else
                {
                    _muteSwitcherImage.sprite = _speakerImage;
                }
                break;
        }
    }
}
