using UnityEngine;
using UnityEngine.EventSystems;

public class TabActiveAnimationSwitcher : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Animator�������AAnimator������Bool�uIsActive�v�����K�v�A��")] GameObject _closeObject;
    Animator _animator;
    bool _isActive = false;
    void Start()
    {
        if(_closeObject)
        {
            try
            {
                _animator = _closeObject.GetComponent<Animator>();
                _isActive = _animator.GetBool("IsActive");
                Debug.Log($"initial state is {_isActive}");
            }
            catch
            {
                Debug.Log("closeObject not contain Animator");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {//animator�����݂��Ă��邩�A�A�j���[�V�������I�����Ă��邩���m�F
        if (_animator &&  _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("start switch tab");
            try
            {
                _animator.SetBool("IsActive", !_isActive);
                _isActive = !_isActive;
            }
            catch
            {
                Debug.Log("animator not contain bool [IsActive]");
            }
        }
    }
    /// <summary>
    /// Button�Ő؂�ւ���p
    /// </summary>
    public void ChangeActiveTab()
    {//animator�����݂��Ă��邩�A�A�j���[�V�������I�����Ă��邩���m�F
        if (_animator && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("start switch tab");
            try
            {
                _animator.SetBool("IsActive", !_isActive);
                _isActive = !_isActive;
            }
            catch
            {
                Debug.Log("animator not contain bool [IsActive]");
            }
        }
    }
}
