using UnityEngine;
using UnityEngine.EventSystems;

public class TabActiveAnimationSwitcher : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Animatorを持ち、Animator内部にBool「IsActive」を持つ必要アリ")] GameObject _closeObject;
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
    {//animatorが存在しているかつ、アニメーションが終了しているかを確認
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
    /// Buttonで切り替える用
    /// </summary>
    public void ChangeActiveTab()
    {//animatorが存在しているかつ、アニメーションが終了しているかを確認
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
