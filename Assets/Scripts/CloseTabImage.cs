using UnityEngine;
using UnityEngine.EventSystems;

public class CloseTabImage : MonoBehaviour, IPointerClickHandler
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
                //animationの終了をまつスクリプトを待つ必要あり
            }
            catch
            {
                Debug.Log("animator not contain bool [IsOpen]");
            }
        }
    }
}
