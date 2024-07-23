using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchTabImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Animatorを持ち、Animator内部にBool「IsOpen」を持つ必要アリ")] GameObject _closeObject;
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
    {
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
