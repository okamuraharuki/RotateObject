using UnityEngine;
using UnityEngine.EventSystems;

public class CloseTabImage : MonoBehaviour, IPointerClickHandler
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
            }
            catch 
            {
                Debug.Log("closeObject not contain Animator");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_animator)
        {
            try
            {
                _animator.SetBool("IsOpen", !_isOpen);
                _isOpen = !_isOpen;
                //animation�̏I�����܂X�N���v�g��҂K�v����
            }
            catch
            {
                Debug.Log("animator not contain bool [IsOpen]");
            }
        }
    }
}
