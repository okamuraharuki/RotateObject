using UnityEngine;
using UnityEngine.EventSystems;

public class TabActiveSwitcher : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Animator�������AAnimator������Bool�uIsOpen�v�����K�v�A��")] GameObject _closeObject;
    Animator _animator;
    bool _isOpen = false;
    void Start()
    {
        if(_closeObject)
        {
            try
            {
                _animator = _closeObject.GetComponent<Animator>();
                _isOpen = _animator.GetBool("IsOpen");
                Debug.Log($"initial state is {_isOpen}");
            }
            catch
            {
                Debug.Log("closeObject not contain Animator");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {//animator�����݂��Ă��邩�A�A�j���[�V�������I�����Ă��邩���m�F
        if (_animator && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("start switch tab");
            try
            {
                _animator.SetBool("IsOpen", !_isOpen);
                _isOpen = !_isOpen;
            }
            catch
            {
                Debug.Log("animator not contain bool [IsOpen]");
            }
        }
    }
}
